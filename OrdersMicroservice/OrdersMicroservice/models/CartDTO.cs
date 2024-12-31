using OrdersMicroservice.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMicroservice.Models
{
    public class CartDTO
    {
        public string id { get; set; }
        public string userId { get; set; }
        public double total { get; set; }
        public ICollection<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
        

        public Order fromCartDTOtoOrder(CartDTO cartDTO)
        {
            return new Order
            {
                costumerId = cartDTO.userId,
                OrderDate = System.DateTime.Now,
                totalPrice = cartDTO.total,
                items = cartDTO.CartItems.Select(x => new OrderItem
                {
                    productId = x.productId,
                    quantity = x.quantity
                }).ToList()
            };
        }


    }
}
