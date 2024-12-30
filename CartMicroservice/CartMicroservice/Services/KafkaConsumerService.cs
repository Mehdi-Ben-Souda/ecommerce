using CartMicroservice.Models;
using Confluent.Kafka;
using System.Text.Json;

namespace OrdersMicroservice.services
{
    public class KafkaConsumerService : IHostedService
    {
       
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool _consuming = false;



        public KafkaConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory )
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "order-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
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

                    if (consumeResult.Topic == "product_info")
                    {
                        var productEvent = JsonSerializer.Deserialize<Product>(consumeResult.Message.Value);
                        //await HandleProductUpdate(productEvent);
                        
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



        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consuming = false;
            _consumer.Close();
            return Task.CompletedTask;
        }
    }
}
