using System;
using System.IO;
using EasyNetQ;
using Micro.DDD.ETLWorker.HttpAPI;
using Micro.DDD.ETLWorker.Utils;
using Micro.DDD.Messages.Messages;
using Microsoft.Extensions.Configuration;
using WebApiClient;

namespace Micro.DDD.ETLWorker
{
    class Program
    {
        private static Workers.EtlWorker _etlWorker;
        private static string _messageAPiHost;
        private static IBus _bus;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            MongoDbUtil mongo = new MongoDbUtil(config["mongo"], config["collection_name"], config["database_name"]);
            _etlWorker = new Workers.EtlWorker(mongo, config["mysql_connection_string"]);
            _messageAPiHost = config["message_api_host"];
            _bus = RabbitHutch.CreateBus(config["mq_url"]);
            _bus.Subscribe<ETLTaskMessage>("shell.etl.worker", ETLTaskHandler, x=>x.WithTopic(config["mq_etl_topic"]));
        }

        private static void ETLTaskHandler(ETLTaskMessage message)
        {
            Console.WriteLine($"{DateTime.Now}: ETLWorker started for {message.VillageName}.");
            _etlWorker.EtlWorkerTask(message.VillageName);
            CallRefreshEvent("Success");
            Console.WriteLine($"{DateTime.Now}: ETLWorker finished for {message.VillageName}.");
        }

        private static void CallRefreshEvent(string message)
        {
            var config = new HttpApiConfig
            {
                HttpHost = new Uri(_messageAPiHost)
            };
            using var client = HttpApi.Create<IMessageClient>(config);
            var info = client.TriggerRefreshEvent(message).GetAwaiter().GetResult();
            Console.WriteLine($"{DateTime.Now}: triggered Call Refresh Event through Http API Call, result is {info}");
        }

    }
}