using Confluent.Kafka;
using Kafka.Dto;
using Kafka.Serailizers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        private readonly KafkaDependentProducer<Null, string> _producer;
        private readonly KafkaExtendedDependentProducer<string, SomeDto> _producer2;

        //private static readonly ProducerConfig config = new ProducerConfig
        //{
        //    BootstrapServers = "localhost:9092",
        //    //  MessageTimeoutMs =5000,
        //   // Acks = Acks.None,
        //    //  CompressionLevel = Com

        //};

        //private static readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(config).Build();
        private readonly string topic = "simpletalk_topic";
        private readonly string _topic2 = "topic2";

        public KafkaProducerController(KafkaDependentProducer<Null, string> producer, KafkaExtendedDependentProducer<string, SomeDto> producer2)
        {
            _producer = producer;
            _producer2 = producer2;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string message)
        {
            //using (var producer =
            //     new ProducerBuilder<Null, string>(config).Build())
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = $"{message} {DateTime.Now}" });
            }
            return Ok();

        }

        [HttpPost("PostTopic2")]
        public async Task<IActionResult> PostToTopic2([FromQuery] SomeDto message)
        {
            await _producer2.ProduceAsync(_topic2, new Message<string, SomeDto>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            return Ok();

        }



        [HttpPost("PostToPartition1")]
        public async Task<IActionResult> Post1([FromQuery] string message)
        {
            var tp = new TopicPartition(topic, new Partition(1));
            {
                await _producer.ProduceAsync(tp, new Message<Null, string> { Value = $"{message} {DateTime.Now}" });
            }
            return Ok();

        }

        [HttpPost("PostToPartition2")]
        public async Task<IActionResult> Post2([FromQuery] string message)
        {
            var tp = new TopicPartition(topic, new Partition(2));
            {
                await _producer.ProduceAsync(tp, new Message<Null, string> { Value = $"{message} {DateTime.Now}" });
            }
            return Ok();

        }

    }
}
