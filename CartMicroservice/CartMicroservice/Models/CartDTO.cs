namespace CartMicroservice.Models
{
    public class CartDTO
    {
        public string? id { get; set; }
        public string? userId { get; set; }
        public double total { get; set; }
        public ICollection<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        public static CartDTO FromCart(Cart cart)
        {
            return new CartDTO
            {
                id = cart.id,
                userId = cart.userId,
                total = cart.total,
                CartItems = cart.CartItems.Select(CartItemDTO.FromCartItem).ToList()
            };
        }
        /*
         public static CartDTO FromCart(Cart cart)
        {
            var tmp = new List<CartItemDTO>();
            foreach (var item in cart.CartItems)
            {
                tmp.Add(CartItemDTO.FromCartItem(item));
            }
            return new CartDTO
            {
                id = cart.id,
                userId = cart.userId,
                total = cart.total,
                CartItems = tmp
            };
        }
         
         */


    }
}
