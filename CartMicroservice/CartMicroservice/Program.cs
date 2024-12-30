using CartMicroservice;
using CartMicroservice.Models;
using CartMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{

    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    {
        ConnectTimeout = 5000,
        ConnectRetry = 3,
        AbortOnConnectFail = false  // This allows the app to continue even if Redis is temporarily down
    };
    // This is the crucial part - we need to explicitly add the endpoint
    options.ConfigurationOptions.EndPoints.Add("localhost", 6379);

    
});

builder.Services.AddSingleton<KafkaProducerService>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");


//___________________ ******* Cart routes ******* ___________________//


// Get a cart by ID
app.MapGet("/Carts/{id}", async (int id, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{id}");
    if (cartJson == null)
    {
        return Results.NotFound();
    }

    var cart = JsonSerializer.Deserialize<Cart>(cartJson);
    return Results.Ok(cart);
});


// Add a new cart
app.MapPost("/Carts", async (Cart cart, [FromServices] IDistributedCache cache) =>
{
    var cartJson = JsonSerializer.Serialize(cart);
    await cache.SetStringAsync($"Cart_{cart.userId}", cartJson);

    return Results.Created($"/Carts/{cart.userId}", cart);
});

//Adding a product to a cart
app.MapPost("/AddToCart", async (CartItem cartItem, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{cartItem.cartId}");
    if (cartJson == null)
    {
        return Results.NotFound("Cart not found");
    }
    var userCart = JsonSerializer.Deserialize<Cart>(cartJson);
    if(userCart == null)
    {
        return Results.NotFound("Cart Empty ( not intanciated )");
    }

    var existingItem = userCart.CartItems.FirstOrDefault(i => i.productId == cartItem.productId);
    if (existingItem != null)
    {
        existingItem.quantity += cartItem.quantity;
    }
    else
    {
        userCart.CartItems.Add(cartItem);
    }

    cartJson = JsonSerializer.Serialize(userCart);
    await cache.SetStringAsync($"Cart_{userCart.id}", cartJson);

    return Results.Ok(cartItem);
});

//Removing a product from a cart
app.MapPost("/RemoveFromCart", async (CartItem cartItem, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{cartItem.cartId}");
    if (cartJson == null)
    {
        return Results.NotFound("Cart not found");
    }
    var userCart = JsonSerializer.Deserialize<Cart>(cartJson);

    var existingItem = userCart.CartItems.FirstOrDefault(i => i.productId == cartItem.productId);
    if (existingItem != null)
    {
        userCart.CartItems.Remove(existingItem);
    }

    cartJson = JsonSerializer.Serialize(userCart);
    await cache.SetStringAsync($"Cart_{userCart.id}", cartJson);

    return Results.Ok(cartItem);
});

//Validate cart ( the cart becomes an order , we publish this in kafka topic and the order microService store the oder)
app.MapPost("/ValidateCart/{id}", async (int id, [FromServices] IDistributedCache cache, [FromServices] KafkaProducerService kafkaProducerService) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{id}");
    if (cartJson == null)
    {
        return Results.NotFound("Cart not found");
    }
    var userCart = JsonSerializer.Deserialize<Cart>(cartJson);

    if (userCart == null  || userCart.CartItems.Count == 0)
    {
        return Results.BadRequest("Cart is empty");
    }
    var cartDTO = CartDTO.FromCart(userCart);

    // Publish the cart to Kafka
    await kafkaProducerService.ProduceAsync("cart_validation", "order", cartDTO);

    // Clear the cart
    userCart.CartItems.Clear();
    userCart.total = 0;
    cartJson = JsonSerializer.Serialize(userCart);
    await cache.SetStringAsync($"Cart_{userCart.id}", cartJson);

    return Results.Ok(userCart);
});

app.Run();
