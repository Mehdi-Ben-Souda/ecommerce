using System.ComponentModel.DataAnnotations;

namespace CartMicroservice.Models
{
    public class Product
    {
      
        public string id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    

    }
}
