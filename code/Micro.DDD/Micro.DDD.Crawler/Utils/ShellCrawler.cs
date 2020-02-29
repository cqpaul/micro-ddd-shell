/**
*@Project: Micro.DDD.Crawler
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 9:54:36 AM
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using Micro.DDD.Crawler.Models;

namespace Micro.DDD.Crawler.Utils
{
    public class ShellCrawler
    {
        private readonly HtmlWeb _web;

        public ShellCrawler(HtmlWeb web)
        {
            _web = web;
        }

        private string BuildCrawlerUrl(int pageNumber, string villageName)
        {
            string baseUrl = $"https://cq.ke.com/ershoufang/pg{pageNumber}rs{villageName}/";
            return baseUrl;
        }

        private string BuildSubwayUrl(int pageNumber, string shellLineNumber)
        {
            string baseUrl = $"https://cq.ke.com/ditiefang/{shellLineNumber}/pg{pageNumber}/";
            return baseUrl;
        }
        
        private int GetTotalSellNumber(string villageName)
        {
            string firstPageUrl = BuildCrawlerUrl(1,villageName);
            var doc = _web.Load(firstPageUrl);            
            var countNode = doc.DocumentNode.SelectNodes("//h2[@class='total fl']/span");
            var totalCount = countNode.First().InnerHtml.Trim();
            int countNumber = Convert.ToInt32(totalCount);
            return countNumber;
        }

        private int GetTotalShellNumberForSubwayType(string shellLineNumber)
        {
            string firstPageUrl = BuildSubwayUrl(1,shellLineNumber);
            var doc = _web.Load(firstPageUrl);            
            var countNode = doc.DocumentNode.SelectNodes("//h2[@class='total fl']/span");
            var totalCount = countNode.First().InnerHtml.Trim();
            int countNumber = Convert.ToInt32(totalCount);
            return countNumber;
        }

        private ShellPageModel CrawlOnePageInfo(string pageUrl)
        {
            ShellPageModel pageInfoModel = new ShellPageModel();
            var doc = _web.Load(pageUrl);
            var itemTitles = doc.DocumentNode.SelectNodes("//a[@class='img VIEWDATA CLICKDATA maidian-detail']");
            pageInfoModel.Titles = itemTitles.Select(node => node.Attributes["title"].Value).ToList();
            var linkUrls = doc.DocumentNode.SelectNodes("//a[@class='img VIEWDATA CLICKDATA maidian-detail']");
            pageInfoModel.LinkUrls = linkUrls.Select(node => node.Attributes["href"].Value).ToList();
            var positions = doc.DocumentNode.SelectNodes("//div[@class='positionInfo']");
            pageInfoModel.Positions = positions.Select(node => node.InnerText.Trim()).ToList();
            var houseInfos = doc.DocumentNode.SelectNodes("//div[@class='houseInfo']");
            List<string> floor = new List<string>();
            List<string> yearInfo = new List<string>();
            List<string> areaStr = new List<string>();
            List<string> areaNumber = new List<string>();
            List<string> orientation = new List<string>();
            foreach (HtmlNode node in houseInfos)
            {
                string allInfo = node.InnerText;
                string[] infoArray = allInfo.Split('|');
                if (infoArray.Length != 5)
                {
                    floor.Add(null);
                    yearInfo.Add(null);
                    areaStr.Add(null);
                    areaNumber.Add(null);
                    orientation.Add(null);
                }
                else
                {
                    floor.Add(infoArray[0].Trim());
                    yearInfo.Add(infoArray[1].Trim());
                    areaStr.Add(infoArray[2].Trim());
                    areaNumber.Add(infoArray[3].Trim());
                    orientation.Add(infoArray[4].Trim());
                }
            }
            var followInfos = doc.DocumentNode.SelectNodes("//div[@class='followInfo']");
            List<string> followNumber = new List<string>();
            List<string> followDay = new List<string>();
            foreach (HtmlNode node in followInfos)
            {
                string followStr = node.InnerText;
                string[] infoArray = followStr.Split('/');
                followNumber.Add(infoArray[0].Trim());
                followDay.Add(infoArray[1].Trim());
            }
            var totalPrices = doc.DocumentNode.SelectNodes("//div[@class='totalPrice']");
            pageInfoModel.Prices = totalPrices.Select(node => node.InnerText.Trim()).ToList();
            var unitPrices = doc.DocumentNode.SelectNodes("//div[@class='unitPrice']");
            pageInfoModel.UnitPrices = unitPrices.Select(node => node.InnerText.Trim()).ToList();
            pageInfoModel.Floors = floor;
            pageInfoModel.YearInfos = yearInfo;
            pageInfoModel.AreaStrings = areaStr;
            pageInfoModel.AreaNumbers = areaNumber;
            pageInfoModel.Orientations = orientation;
            pageInfoModel.FollowNumbers = followNumber;
            pageInfoModel.FollowDays = followDay;
            return pageInfoModel;
        }

        private IEnumerable<ShellNode> ShellModelToNodes(ShellPageModel shellPageModel, string villageName)
        {
            List<ShellNode> nodes = new List<ShellNode>();
            int count = shellPageModel.Floors.Count();
            for (int i = 0; i < count; i++)
            {
                ShellNode node = new ShellNode();
                node.VillageName = villageName;
                node.Position = shellPageModel.Positions[i];
                node.Floor = shellPageModel.Floors[i];
                node.Orientation = shellPageModel.Orientations[i];
                node.Price = shellPageModel.Prices[i];
                node.Title = shellPageModel.Titles[i];
                node.AreaNumber = shellPageModel.AreaNumbers[i];
                node.AreaStr = shellPageModel.AreaStrings[i];
                node.FollowDay = shellPageModel.FollowDays[i];
                node.FollowNumber = shellPageModel.FollowNumbers[i];
                node.UnitPrice = shellPageModel.UnitPrices[i];
                node.YearInfo = shellPageModel.YearInfos[i];
                node.CrawlDate = DateTime.Now;
                node.LinkUrl = shellPageModel.LinkUrls[i];
                nodes.Add(node);
            }
            
            return nodes;
        }

        public IEnumerable<ShellNode> StartCrawlForOneVillage(string villageName)
        {
            List<ShellNode> shellNodes = new List<ShellNode>();
            int totalNumber = GetTotalSellNumber(villageName);
            int pages = totalNumber / 30;
            if (totalNumber % 30 > 0)
            {
                pages += 1;
            }
            Console.WriteLine($"{DateTime.Now}: Total page: {pages}");
            for (int i = 1; i <= pages; i++)
            {
                string pageUrl = BuildCrawlerUrl(i, villageName);
                ShellPageModel pageModel = CrawlOnePageInfo(pageUrl);
                Console.WriteLine($"{DateTime.Now}: start to crawl page: {pageUrl}.");
                IEnumerable<ShellNode> pageNodes = ShellModelToNodes(pageModel, villageName);
                shellNodes.AddRange(pageNodes);
                Thread.Sleep(1000);
            }

            return shellNodes;
        }

        public IEnumerable<ShellNode> StartCrawlForSubwayRoom(string shellLineNumber, string shellLineName)
        {
            List<ShellNode> shellNodes = new List<ShellNode>();
            int totalNumber = GetTotalShellNumberForSubwayType(shellLineNumber);
            int pages = totalNumber / 30;
            if (totalNumber % 30 > 0)
            {
                pages += 1;
            }
            Console.WriteLine($"{DateTime.Now}: Total page: {pages}.");
            if (pages > 100)
            {
                pages = 100; // Only show 100 pages.
            }

            for (int i = 1; i <= pages; i++)
            {
                string pageUrl = BuildSubwayUrl(i, shellLineNumber);
                ShellPageModel pageModel = CrawlOnePageInfo(pageUrl);
                Console.WriteLine($"{DateTime.Now}: <{shellLineName}> start to crawl page: {pageUrl}.");
                IEnumerable<ShellNode> pageNodes = ShellModelToNodes(pageModel, shellLineName);
                shellNodes.AddRange(pageNodes);
                Thread.Sleep(1000);
            }
            return shellNodes;
        }

    }
}