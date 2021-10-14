using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string topic = "simpletalk_topic";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Обязательно запускаем в фоне, иначе будет висеть бесконечно builder.Consume
            Task.Run(() => StartConsumerLoop(stoppingToken));

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken stoppingToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group3",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                //  EnableAutoCommit = true

            };

            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build())
            {
                builder.Subscribe(topic);

                while (!stoppingToken.IsCancellationRequested)
                {

                    ConsumeResult<Ignore, string> consumeResult = builder.Consume(stoppingToken);

                    // builder.Commit();
                    if (consumeResult.IsPartitionEOF)
                    {
                        Console.WriteLine(
                            $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                        continue;
                    }

                    if (consumeResult?.Message != null)
                        Console.WriteLine($"Message: {consumeResult.Message.Value} received from {consumeResult.TopicPartitionOffset}");
                }


            }
        }
    }
}
