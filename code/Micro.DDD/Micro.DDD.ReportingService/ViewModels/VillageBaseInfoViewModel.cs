/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Friday, December 20, 2019 1:40:11 PM
*/

namespace Micro.DDD.ReportingService.ViewModels
{
    public class VillageBaseInfoViewModel
    {
        public string VillageName { get; set; }
        public int SellCount { get; set; }
        public int BuildingYears { get; set; }
        public double AveragePrice { get; set; }
        public double AverageTotalPrice { get; set; }
        public double TopPrice { get; set; }
        public double LowestPrice { get; set; }
        public ShellNodeViewModel TopNode { get; set; }
        public ShellNodeViewModel LowestNode { get; set; }
    }
}