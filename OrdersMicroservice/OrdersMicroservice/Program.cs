using OrdersMicroservice;
using OrdersMicroservice.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MongoDbConfig>();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("Order/all",(OrderService orderService) =>
{
    return Results.Ok(orderService.GetOrders());
});

app.MapGet("Order/{userId}", (OrderService orderService, int userId) =>
{
    return Results.Ok(orderService.GetOrdersByUserId(userId));
});

app.UseHttpsRedirection();



app.Run();
