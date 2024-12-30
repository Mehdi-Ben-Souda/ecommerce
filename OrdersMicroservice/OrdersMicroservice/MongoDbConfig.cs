using MongoDB.Driver;
using OrdersMicroservice.models;

namespace OrdersMicroservice
{
    public class MongoDbConfig
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;

        public MongoDbConfig(IConfiguration configuration)
        {
            _client = new MongoClient(
                configuration.GetValue<string>("OrderDatabase:ConnectionString"));
            _database = _client.GetDatabase(
                configuration.GetValue<string>("OrderDatabase:DatabaseName"));
        }

        // This is how we register our collections
        public IMongoCollection<Order> Orders =>
            _database.GetCollection<Order>("Orders");
    }
}

