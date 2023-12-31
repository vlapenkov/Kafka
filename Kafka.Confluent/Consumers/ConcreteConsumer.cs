using Kafka.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Consumers
{
    public class ConcreteConsumer : BaseConsumer<string,SomeDto>
    {
        public ConcreteConsumer(IConfiguration configuration):base(configuration)
        {

        }

        public override string Topic => "topic2";

        public override Task Handle(SomeDto value)
        {
            Console.WriteLine(value);
            return Task.CompletedTask;
        }

      
    }
}
