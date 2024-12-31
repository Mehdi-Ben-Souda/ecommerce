namespace OrdersMicroservice.models
{
    public class OrderItem
    {
        public string productId { get; set; }
        public int quantity { get; set; }
        public float totalPrice { get; set; }
    }
}
