using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace OrdersMicroservice.models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string costumerId { get; set; }
        public DateTime OrderDate { get; set; }

        public double totalPrice { get; set; }
        public List<OrderItem> items { get; set; }
    }
}
