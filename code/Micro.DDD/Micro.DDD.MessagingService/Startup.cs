using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Micro.DDD.Common.Consul;
using Micro.DDD.MessingService.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Micro.DDD.MessingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options => options.AddPolicy("CorsPolicy", 
                builder => 
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins("http://localhost:3000", "http://localhost:5000")
                        .AllowCredentials();
                }));
            
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHostApplicationLifetime lifetime)
        {
            // CORS
            app.UseCors("CorsPolicy");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/messageHub");
                endpoints.MapControllers(); 
            });
            
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