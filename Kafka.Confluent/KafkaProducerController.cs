using Confluent.Kafka;
using Kafka.Dto;
using Kafka.Producers;
using Kafka.Serailizers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        // старая реализация
        private readonly KafkaDependentProducer<Null, string> _producer;        

        private readonly IKafkaProducer<string, SomeDto> _producer2;

        private readonly IKafkaProducer<string, ExtendedDto> _producer3;


        private readonly string topic = "simpletalk_topic";


        public KafkaProducerController(
            //KafkaDependentProducer<Null, string> producer,
            IKafkaProducer<string, SomeDto> producer2,
            IKafkaProducer<string, ExtendedDto> producer3)
        {
            //_producer = producer;
            _producer = null;
            _producer2 = producer2;
            _producer3 = producer3;
        }
       

        //[HttpPost]
        //public async Task<IActionResult> Post([FromQuery] string message)
        //{          
        // await _producer.ProduceAsync(topic, new Message<Null, string> { Value = $"{message} {DateTime.Now}" });
         
        //    return Ok();

        //}

        [HttpPost("PostTopic2")]
        public async Task<IActionResult> PostToTopic2([FromQuery] SomeDto message)
        {
            await _producer2.ProduceAsync("topic2", new Message<string, SomeDto>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            return Ok();

        }

        [HttpPost("PostTopic3")]
        public async Task<IActionResult> PostToTopic3([FromQuery] ExtendedDto message)
        {
            await _producer3.ProduceAsync("topic3", new Message<string, ExtendedDto>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            return Ok();

        }



        #region PostToPartitions
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

        #endregion
    }
}
