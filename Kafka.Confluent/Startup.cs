using Confluent.Kafka;
using Kafka.Consumers;
using Kafka.Consumers.Obsolete;
using Kafka.Dto;
using Kafka.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHostedService<KafkaConsumerService>();





            services.AddKafkaConsumer<Topic2Consumer>();
            services.AddKafkaConsumer<Topic3Consumer>();
                       


            //services.AddSingleton<KafkaClientHandle>();
            //services.AddSingleton<KafkaDependentProducer<Null, string>>();

            services.AddKafkaProducer<string, SomeDto>();
            services.AddKafkaProducer<string, ExtendedDto>();

            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kafka producer", Version = "v1" });
            });

         
            // services.AddSingleton<IHostedService, KafkaConsumerHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kafka v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
