using Confluent.Kafka;
using Kafka.Dto;
using Kafka.Serailizers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka
{
    public abstract class BaseHostedConsumer<K, V> : IHostedService
    {
        private readonly IConsumer<K, V> _kafkaConsumer;      
             

        public abstract string Topic { get; }

        public BaseHostedConsumer(IConfiguration config)
        {
            var conf = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(conf);


            _kafkaConsumer = new ConsumerBuilder<K, V>(conf)
                    .SetValueDeserializer(new GenericSerializer<V>())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
        }

        public abstract Task Handle( V value);

        

        protected async Task StartConsumerLoop(CancellationToken cancellationToken)
        {

            _kafkaConsumer.Subscribe(Topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _kafkaConsumer.Consume(cancellationToken);

                    if (consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    var res = consumeResult.Message.Value;

                    await Handle(res);

                    _kafkaConsumer.Commit(consumeResult);
                }
                catch (KafkaException e)
                {
                    Console.WriteLine($"Commit error: {e.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    //_logger.LogError(e, $"Unexpected error: {e}");
                    //break;
                }

            }
            
        }  

    
        

        public void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
             Task.Run(() => StartConsumerLoop(cancellationToken)).ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
