using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuthentificationMicroservice.models
{
    public class appUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

    }
}
