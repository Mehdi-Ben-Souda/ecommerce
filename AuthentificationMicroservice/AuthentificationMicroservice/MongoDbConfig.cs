using AuthentificationMicroservice.models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace AuthentificationMicroservice
{
    public class MongoDbConfig : DbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbConfig(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("CartDatabase");
        }

        public IMongoCollection<appUser> AppUsers => _database.GetCollection<appUser>("Users");
    }
}
