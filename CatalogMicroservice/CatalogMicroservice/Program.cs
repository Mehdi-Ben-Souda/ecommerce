using CatalogMicroservice.models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Products", async (ApplicationDbContext db) =>
    await db.Products.Find(_ => true).ToListAsync());


app.MapGet("/Products/{id}", async (string id, ApplicationDbContext db) =>
    await db.Products.Find(p => p.id == id).FirstOrDefaultAsync());

app.MapPost("/Products", async (Product product, ApplicationDbContext db) =>
{
    await db.Products.InsertOneAsync(product);
    return Results.Created($"/Products/{product.id}", product);
});


app.Run();
