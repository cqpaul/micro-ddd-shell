using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Micro.DDD.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hosting, builder) => { builder.SetBasePath(hosting.HostingEnvironment.ContentRootPath).AddJsonFile("ocelot_direct.json"); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseUrls("http://*:5000").UseStartup<Startup>(); });
    }
}