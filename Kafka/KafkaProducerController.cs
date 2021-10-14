using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //private static readonly ProducerConfig config = new ProducerConfig
        //{
        //    BootstrapServers = "localhost:9092",
        //    //  MessageTimeoutMs =5000,
        //   // Acks = Acks.None,
        //    //  CompressionLevel = Com

        //};

        //private static readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(config).Build();
        private readonly string topic = "simpletalk_topic";

        public KafkaProducerController(KafkaDependentProducer<Null, string> producer)
        {
            _producer = producer;
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

    }
}
