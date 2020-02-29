/**
*@Project: Micro.DDD.Common
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 3:04:00 PM
*/

using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Micro.DDD.Common.EasyNetQ
{
    public static class EasyNetQExtensions
    {
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder app, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = app.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = services.GetService<IHostApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                subscriber.Subscribe(assembly);
                subscriber.SubscribeAsync(assembly);
            });
            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); });
            return app;
        }
    }
}