/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Friday, December 20, 2019 1:44:34 PM
*/

using System;

namespace Micro.DDD.ReportingService.ViewModels
{
    public class ShellNodeViewModel
    {
        public string _id { get; set; }
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