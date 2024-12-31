using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrdersMicroservice.models;

namespace OrdersMicroservice.services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;
        public OrderService(MongoDbConfig config)
        {
            _orders = config.Orders;
        }

        //add a new order
        public Order AddOrder(Order order)
        {
            _orders.InsertOne(order);
            return order;
        }

        //get order by user id
        public List<Order> GetOrdersByUserId(string userId)
        {
            return _orders.Find(order => order.costumerId == userId).ToList();
        }

        //get all orders
        public List<Order> GetOrders()
        {
            return _orders.Find(order => true).ToList();
        }
    }
}
