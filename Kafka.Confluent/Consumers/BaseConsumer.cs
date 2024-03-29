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

namespace Kafka.Consumers
{

    public abstract class BaseConsumer : BackgroundService { }
    public abstract class BaseConsumer<K, V> : BaseConsumer
    {
        private readonly IConsumer<K, V> _kafkaConsumer;


        public abstract string Topic { get; }

        public BaseConsumer(IConfiguration config)
        {
            var conf = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(conf);


            _kafkaConsumer = new ConsumerBuilder<K, V>(conf)
                    .SetValueDeserializer(new GenericSerializer<V>())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
        }

        public abstract Task Handle(V value);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

           await Task.Run(() => StartConsumerLoop(stoppingToken));

            //return Task.CompletedTask;
        }

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

                    // _kafkaConsumer.Commit(consumeResult);

                    _kafkaConsumer.StoreOffset(consumeResult);
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




        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
            base.Dispose();
        }
    }
}
