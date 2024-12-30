using Confluent.Kafka;
using System.Text.Json;

namespace CartMicroservice.Services
{
    public class KafkaProducerService
    {
        
        private readonly IProducer<string, string> _producer;

        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string topic, string key, T message)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(message);
                var kafkaMessage = new Message<string, string>
                {
                    Key = key,
                    Value = jsonMessage
                };

                var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);
                Console.WriteLine($"Delivered message to {deliveryResult.TopicPartitionOffset}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error producing message: {ex.Message}");
                throw;
            }
        }
    }
}
