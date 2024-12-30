using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMicroservice.Models
{
    public class Cart
    {
        [Key]
        public string? id { get; set; }
        public string? userId { get; set; }
        public double total { get; set; }
        [InverseProperty("Cart")] 
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


    }
}
