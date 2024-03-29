using Kafka.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Consumers
{
    public class Topic2Consumer : BaseConsumer<string,SomeDto>
    {
        public Topic2Consumer(IConfiguration configuration):base(configuration)
        {

        }

        public override string Topic => "topic2";

        public override Task Handle(SomeDto value)
        {
            Console.WriteLine($"From topic 2: {value}");
            return Task.CompletedTask;
        }

      
    }
}
