﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka
{
    public class KafkaClientHandle : IDisposable
    {
        IProducer<byte[], byte[]> kafkaProducer;

        public KafkaClientHandle(IConfiguration config)
        {
            var conf = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings").Bind(conf);
            this.kafkaProducer = new ProducerBuilder<byte[], byte[]>(conf).Build();
        }

        public Handle Handle { get => this.kafkaProducer.Handle; }

        public void Dispose()
        {
            // Block until all outstanding produce requests have completed (with or
            // without error).
            kafkaProducer.Flush();
            kafkaProducer.Dispose();
        }
    }
}
