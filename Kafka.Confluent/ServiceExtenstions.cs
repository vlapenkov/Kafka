using Kafka.Consumers;
using Kafka.Dto;
using Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kafka
{
    public static class ServiceExtenstions
    {
        public static void AddKafkaConsumer<TConsumer>(this IServiceCollection services) where TConsumer : BaseConsumer {
            services.AddHostedService<TConsumer>();
        }

        public static void AddKafkaProducer<K,V>(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IKafkaProducer<K, V>), typeof(KafkaExtendedDependentProducer<K, V>));
        }
    }
}
