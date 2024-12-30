using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMicroservice.Models
{
    public class CartItem
    {
        public string cartId { get; set; }
        //public Cart cart { get; set; } = null!;

        public string productId { get; set; }
        public Product product { get; set; } = null!;
        public int quantity { get; set; } 


    }
}
