/**
*@Project: Micro.DDD.Crawler
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 1:34:52 PM
*/

using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Micro.DDD.Crawler.Models;

namespace Micro.DDD.Crawler.Utils
{
    public class Crawler
    {
        private readonly string _dbUrl;
        private readonly string _dbName;
        private readonly string _collectionName;

        public Crawler(string dbUrl, string dbName, string collectionName)
        {
            this._collectionName = collectionName;
            this._dbName = dbName;
            this._dbUrl = dbUrl;
        }

        public void StartCrawler(string villageName)
        {
            HtmlWeb web = new HtmlWeb();
            // 华宇西城丽景
            // 龙湖好城时光
            ShellCrawler shellCrawler = new ShellCrawler(web);

            List<ShellNode> shellNodes = new List<ShellNode>();
            
            // Crawl Villages
            IEnumerable<ShellNode> villageNodes = shellCrawler.StartCrawlForOneVillage(villageName);
            shellNodes.AddRange(villageNodes);
            
            // Crawl Subways -- 2019/12/21
            
            foreach (KeyValuePair<string,string> keyValuePair in Constant.Constants.SubwayMapping)
            {
                IEnumerable<ShellNode> subwayNodes = shellCrawler.StartCrawlForSubwayRoom(keyValuePair.Key, keyValuePair.Value);
                shellNodes.AddRange(subwayNodes);
            }

            MongoDbUtil mongoDbUtil = new MongoDbUtil(_dbUrl, _collectionName, _dbName);
            Console.WriteLine($"{DateTime.Now}: Total nodes {shellNodes.Count}");
            mongoDbUtil.InsertShellNodes(shellNodes);
            Console.WriteLine($"{DateTime.Now}: <{villageName}> finished.");
        }
        
    }
}