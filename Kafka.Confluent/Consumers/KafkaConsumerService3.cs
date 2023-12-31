using Confluent.Kafka;
using Kafka.Dto;
using Kafka.Serailizers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka
{
    public class KafkaConsumerService3 : BackgroundService
    {
        private static readonly string topic = "topic2";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Обязательно запускаем в фоне, иначе будет висеть бесконечно builder.Consume
            Task.Run(() => StartConsumerLoop(stoppingToken));

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken stoppingToken)
        {
            int commitPeriod = 2;

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_fromtopic3",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                //EnableAutoOffsetStore = false,
              

            };

            using (var consumer = new ConsumerBuilder<string, SomeDto>(conf)                
                .SetValueDeserializer(new GenericSerializer<SomeDto>())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build())
            {
                consumer.Subscribe(topic);

                while (!stoppingToken.IsCancellationRequested)
                {

                    ConsumeResult<string, SomeDto> consumeResult = consumer.Consume(stoppingToken);

                    // builder.Commit();
                    if (consumeResult.IsPartitionEOF)
                    {
                        Console.WriteLine(
                            $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                        continue;
                    }


                    if (consumeResult?.Message != null)
                        Console.WriteLine($"Message: {consumeResult.Message.Value} received"+
                            $"partition: {consumeResult.TopicPartition}," +
                            $"offset : {consumeResult.Offset}," +
                            $" key {consumeResult.Key}");



                  //  if (consumeResult.Offset % commitPeriod == 0)
                    {
                        try
                        {
                          //  consumer.Commit(consumeResult);
                            //consumer.StoreOffset(consumeResult);
                        }
                        catch (KafkaException e)
                        {
                            Console.WriteLine($"Commit error: {e.Error.Reason}");
                        }
                    }

                }


            }
        }
    }
}
