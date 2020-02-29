using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Micro.DDD.Common.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Micro.DDD.CrawlerManagerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options=>options.AddPolicy("policy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

            }));
            // Event Bus
            services.AddSingleton(RabbitHutch.CreateBus(Configuration["mq_url"]));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            // CORS
            app.UseCors("policy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            var consulOption = new ConsulOption
            {
                ServiceName = Configuration["ConsulConfig:ServiceName"],
                ServiceIp = Configuration["ConsulConfig:ServiceIP"],
                ServicePort = Convert.ToInt32(Configuration["ConsulConfig:ServicePort"]),
                ServiceHealthCheck = Configuration["ConsulConfig:ServiceHealthCheck"],
                ConsulAddress = Configuration["ConsulConfig:ConsulAddress"]
            };
            app.RegisterConsul(lifetime, consulOption);
            
        }
    }
}