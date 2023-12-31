using Confluent.Kafka;
using Kafka.Serailizers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka
{
    public class KafkaExtendedDependentProducer<K, V> : IDisposable
    {
        IProducer<K, V> kafkaHandle;

        public KafkaExtendedDependentProducer(IConfiguration config)
        {
            //  kafkaHandle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
            var conf = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings").Bind(conf);
           // 

              ProducerBuilder <K,V> builder = new ProducerBuilder<K, V>(conf);

            if (default(V) == null)
                builder= builder.SetValueSerializer(new GenericSerializer<V>());

            this.kafkaHandle = builder.Build();
        }

        public Task ProduceAsync(TopicPartition topicPartition, Message<K, V> message) =>
        this.kafkaHandle.ProduceAsync(topicPartition, message);

        /// <summary>
        ///     Asychronously produce a message and expose delivery information
        ///     via the returned Task. Use this method of producing if you would
        ///     like to await the result before flow of execution continues.
        /// <summary>
        public Task ProduceAsync(string topic, Message<K, V> message)
            => this.kafkaHandle.ProduceAsync(topic, message);

        /// <summary>
        ///     Asynchronously produce a message and expose delivery information
        ///     via the provided callback function. Use this method of producing
        ///     if you would like flow of execution to continue immediately, and
        ///     handle delivery information out-of-band.
        /// </summary>
        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>> deliveryHandler = null)
            => this.kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => this.kafkaHandle.Flush(timeout);

        public void Dispose()
        {
            this.kafkaHandle.Flush();
            this.kafkaHandle.Dispose();
        }
    }
}
