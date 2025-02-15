using Confluent.Kafka;

namespace PhoneBookApi.Services
{
    public class KafkaService
    {
        private readonly string _bootstrapServers;

        public KafkaService(IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"]
                ?? throw new ArgumentNullException(
                    "Kafka bootstrap servers are not configured.");
        }

        public async Task ProduceMessageAsync(string topic, string message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(
                        topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Message sent to topic {topic}: {result.Value}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Error producing message: {e.Error.Reason}");
                }
            }
        }
    }
}