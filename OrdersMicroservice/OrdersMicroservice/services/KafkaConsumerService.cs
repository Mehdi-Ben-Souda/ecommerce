using CartMicroservice.Models;
using Confluent.Kafka;
using OrdersMicroservice.models;
using System.Text.Json;

namespace OrdersMicroservice.services
{
    public class KafkaConsumerService : IHostedService
    {
       
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool _consuming = false;

        private readonly OrderService _orderService;


        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory , OrderService orderService)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "order-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
            _orderService = orderService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(new[] { "cart_validation"});
            _consuming = true;

            Task.Run(() => ConsumeMessages(cancellationToken));
            return Task.CompletedTask;
        }

        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            while (_consuming)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult.Topic == "cart_validation")
                    {
                        var productEvent = JsonSerializer.Deserialize<CartDTO>(consumeResult.Message.Value);
                        //await HandleProductUpdate(productEvent);
                        await HandleCartValidation(productEvent);
                    }

                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error consuming message: {ex.Message}");
                }
            }
        }

        private Task HandleCartValidation(CartDTO? cartdto)
        {
            Console.WriteLine("Recived cart validation event throught cart_validation Topic");
            if(cartdto == null)
            {
                Console.Error.WriteLine("CartDTO is null");
                return Task.CompletedTask;
            }

            _orderService.AddOrder(cartdto.fromCartDTOtoOrder(cartdto));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consuming = false;
            _consumer.Close();
            return Task.CompletedTask;
        }
    }
}
