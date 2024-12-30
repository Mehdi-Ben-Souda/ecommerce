namespace OrdersMicroservice.models
{
    public class OrderItem
    {
        public int productId { get; set; }
        public int quantity { get; set; }
        public float totalPrice { get; set; }
    }
}
