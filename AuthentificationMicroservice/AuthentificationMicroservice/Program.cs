using AuthentificationMicroservice;
using AuthentificationMicroservice.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbConfig>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "Hello World!");

//__________________  authentificaion routes ______________________
app.MapPost("/authentification/login", async (string email, string password , MongoDbConfig db) =>
{
    var user = await db.AppUsers.Find(user => user.email == email && user.password == password).FirstOrDefaultAsync();
    
    if (user != null)
    {
        return Results.Ok(user);
    }
    else
    {
        return Results.BadRequest("login failed");
    }
});

app.MapGet("/authentification/User/{id}", async (string id, MongoDbConfig db) =>
    await db.AppUsers.Find(p => p.id == id).FirstOrDefaultAsync());

app.MapPost("/authentification/register", (string username, string email, string password, MongoDbConfig db) =>
{
    var users = db.AppUsers.Find(user => user.email == email);

    if(users.CountDocuments()>0)
        return Results.BadRequest("register failed , email exists");

    appUser user = new appUser();
    user.username = username;
    user.email = email;
    user.password = password;
    db.AppUsers.InsertOne(user);
    db.SaveChanges();
    return Results.Ok("register success");
});




app.UseHttpsRedirection();


app.Run();
