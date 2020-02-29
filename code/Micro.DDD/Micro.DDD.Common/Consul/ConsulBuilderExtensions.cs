/**
*@Project: Micro.DDD.Common
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 10:04:00 AM
*/

using System;
using System.Collections.Generic;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Micro.DDD.Common.Consul
{
    public static class ConsulBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulOption consulOption)
        {
            var consulClient = new ConsulClient(o =>
            {
                o.Address = new Uri(consulOption.ConsulAddress);
            });
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulOption.ServiceName,
                Address = consulOption.ServiceIp,
                Port = consulOption.ServicePort,
                Meta = new Dictionary<string, string>(),
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = consulOption.ServiceHealthCheck,
                    Timeout = TimeSpan.FromSeconds(10)
                }
            };
            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopped.Register(() => consulClient.Agent.ServiceDeregister(registration.ID).Wait());
            return app;
        }
    }
}