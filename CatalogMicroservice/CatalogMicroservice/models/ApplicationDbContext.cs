using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CatalogMicroservice.models
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMongoDatabase _database;
        public ApplicationDbContext(IConfiguration configuration )
             {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("CartDatabase");
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    }
}
