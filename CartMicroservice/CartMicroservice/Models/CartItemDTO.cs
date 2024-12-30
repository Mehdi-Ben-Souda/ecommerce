namespace CartMicroservice.Models
{
    public class CartItemDTO
    {
        public int productId { get; set; }
        public int quantity { get; set; }


        public static CartItemDTO FromCartItem(CartItem cartItem)
        {
            return new CartItemDTO
            {
                productId = cartItem.productId,
                quantity = cartItem.quantity
            };
        }

    }
}
