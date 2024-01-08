using Kafka.Consumers.Obsolete;
using Kafka.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Consumers
{
    public class Topic3Consumer : BaseConsumer<string,ExtendedDto>
    {
        public Topic3Consumer(IConfiguration configuration):base(configuration)
        {

        }

        public override string Topic => "topic3";

        public override Task Handle(ExtendedDto value)
        {
            Console.WriteLine($"From topic 3: {value}");
            return Task.CompletedTask;
        }

      
    }
}
