using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMicroservice.Models
{
    public class CartItemDTO
    {
        public int productId { get; set; }
        public int quantity { get; set; } 

    }
}
