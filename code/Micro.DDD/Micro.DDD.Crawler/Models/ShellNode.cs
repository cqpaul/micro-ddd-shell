/**
*@Project: Micro.DDD.Crawler
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 9:53:28 AM
*/

using System;

namespace Micro.DDD.Crawler.Models
{
    public class ShellNode
    {
        public string VillageName { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        public string Floor { get; set; }
        public string YearInfo { get; set; }
        public string AreaStr { get; set; }
        public string AreaNumber { get; set; }
        public string Orientation { get; set; }
        public string FollowNumber { get; set; }
        public string FollowDay { get; set; }
        public string Price { get; set; }
        public string UnitPrice { get; set; }
        public DateTime CrawlDate { get; set; }
        public string LinkUrl { get; set; }
    }
}