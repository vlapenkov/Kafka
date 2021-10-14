using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka
{
    public class TestHostedService : BackgroundService
    {


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Logic here
                await Task.Delay(10000, stoppingToken);
                Console.WriteLine("TestHostedService");
            }
        }
    }
}
