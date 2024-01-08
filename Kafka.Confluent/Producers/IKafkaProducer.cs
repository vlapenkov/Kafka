using Confluent.Kafka;
using System.Threading.Tasks;

namespace Kafka.Producers
{
    public interface IKafkaProducer<K, V>
    {
        Task ProduceAsync(string topic, Message<K, V> message);
    }
}
