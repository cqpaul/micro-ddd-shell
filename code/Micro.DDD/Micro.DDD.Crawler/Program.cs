using System;
using System.IO;
using EasyNetQ;
using Micro.DDD.Messages.Messages;
using Microsoft.Extensions.Configuration;

namespace Micro.DDD.Crawler
{
    class Program
    {
        private static Utils.Crawler _crawler;
        private static IBus _bus;
        private static string _etlTopic;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            _crawler = new Utils.Crawler(config["mongo"],config["database_name"],config["collection_name"]);
            _bus = RabbitHutch.CreateBus(config["mq_url"]);
            _etlTopic = config["mq_etl_topic"];
            _bus.Subscribe<NewCrawlTaskMessage>("shell.crawler", TriggerCrawlTaskHandler,x=>x.WithTopic(config["mq_crawl_topic"]));
        }

        private static void TriggerCrawlTaskHandler(NewCrawlTaskMessage message)
        {
            Console.WriteLine($"{DateTime.Now}: Crawler started for {message.CrawlVillageName}.");
            _crawler.StartCrawler(message.CrawlVillageName);
            // Publish ETL Task Event
            PublishEtlMessage(message.CrawlVillageName);
            Console.WriteLine($"{DateTime.Now}: Crawler finished for {message.CrawlVillageName}.");
        }
        
        private static void PublishEtlMessage(string villageName)
        {
            Console.WriteLine($"{DateTime.Now}: Crawler publish etl task for {villageName}.");
            ETLTaskMessage etlTaskMessage = new ETLTaskMessage {ETLStartTask = true, VillageName = villageName};
            _bus.Publish(etlTaskMessage, _etlTopic);
        }

    }
}