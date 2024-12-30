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
    options.ConfigurationOptions.EndPoints.Add("redisDB", 6379);

    
});

builder.Services.AddSingleton<KafkaProducerService>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");


//___________________ ******* Cart routes ******* ___________________//


// Get a cart by ID
app.MapGet("/Cart/{id}", async (string id, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{id}");
    if (cartJson == null)
    {
        return Results.NotFound();
    }

    var cart = JsonSerializer.Deserialize<Cart>(cartJson);
    // for each cartItem in cart, get the product from the product microservice uing the the api endponit
    // and add it to the cartItem
    // Create a single HttpClient instance
    using var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("http://catalogMicroService");

    // Create a list to store our tasks
    var productTasks = cart.CartItems.Select(async cartItem =>
    {
        try
        {
            // Make the HTTP request to get the product
            var response = await httpClient.GetAsync($"/Products/{cartItem.productId}");

            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                return (cartItem, JsonSerializer.Deserialize<Product>(productJson));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching product {cartItem.productId}: {ex.Message}");
        }
        return (cartItem, null);
    });

    // Wait for all requests to complete
    var results = await Task.WhenAll(productTasks);

    // Update cart items with their products
    foreach (var (cartItem, product) in results.Where(r => r.Item2 != null))
    {
        // Since we already have the CartItem model set up with a Product property
        // we can just assign it
        cartItem.product = product;
    }

    return Results.Ok(cart);
});


// Add a new cart
app.MapPost("/Cart", async (Cart cart, [FromServices] IDistributedCache cache) =>
{
    var cartJson = JsonSerializer.Serialize(cart);
    await cache.SetStringAsync($"Cart_{cart.userId}", cartJson);

    return Results.Created($"/Cart/{cart.userId}", cart);
});

//Adding a product to a cart
app.MapPost("/Cart/AddToCart", async (CartItem cartItem, [FromServices] IDistributedCache cache) =>
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

//Updating a product in a cart
app.MapPut("/Cart/UpdateCart", async (CartItem cartItem, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{cartItem.cartId}");
    if (cartJson == null)
    {
        return Results.NotFound("Cart not found");
    }
    var userCart = JsonSerializer.Deserialize<Cart>(cartJson);
    if (userCart == null)
    {
        return Results.NotFound("Cart Empty ( not instanciated )");
    }

    var existingItem = userCart.CartItems.FirstOrDefault(i => i.productId == cartItem.productId);
    if (existingItem != null)
    {
        existingItem.quantity = cartItem.quantity;
    }

    cartJson = JsonSerializer.Serialize(userCart);
    await cache.SetStringAsync($"Cart_{userCart.id}", cartJson);

    return Results.Ok(cartItem);
});

//Removing a product from a cart
app.MapPost("/Cart/RemoveFromCart", async (CartItem cartItem, [FromServices] IDistributedCache cache) =>
{
    var cartJson = await cache.GetStringAsync($"Cart_{cartItem.cartId}");
    if (cartJson == null)
    {
        return Results.NotFound("Cart not found");
    }
    var userCart = JsonSerializer.Deserialize<Cart>(cartJson);
    if (userCart == null)
    {
        return Results.NotFound("Cart Empty ( not instanciated )");
    }

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
app.MapPost("/Cart/ValidateCart/{id}", async (string id, [FromServices] IDistributedCache cache, [FromServices] KafkaProducerService kafkaProducerService) =>
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
