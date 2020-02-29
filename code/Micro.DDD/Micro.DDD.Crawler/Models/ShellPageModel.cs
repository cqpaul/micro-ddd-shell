/**
*@Project: Micro.DDD.Crawler
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 9:53:44 AM
*/

using System.Collections.Generic;

namespace Micro.DDD.Crawler.Models
{
    public class ShellPageModel
    {
        public List<string> Titles { get; set; }
        public List<string> Floors { get; set; }
        public List<string> YearInfos { get; set; }
        public List<string> AreaStrings { get; set; }
        public List<string> AreaNumbers { get; set; }
        public List<string> Orientations { get; set; }
        public List<string> FollowNumbers { get; set; }
        public List<string> FollowDays { get; set; }
        public List<string> Prices { get; set; }
        public List<string> UnitPrices { get; set; }
        public List<string> Positions { get; set; }
        public List<string> LinkUrls { get; set; }
    }
}