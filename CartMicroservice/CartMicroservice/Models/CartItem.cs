using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMicroservice.Models
{
    public class CartItem
    {
        public int cartId { get; set; }
        public Cart cart { get; set; } = null!;

        public int productId { get; set; }
        public Product product { get; set; } = null!;
        public int quantity { get; set; } 


    }
}
