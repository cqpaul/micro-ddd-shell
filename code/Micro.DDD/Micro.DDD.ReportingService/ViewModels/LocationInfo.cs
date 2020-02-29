/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Sunday, December 29, 2019 5:58:05 PM
*/

namespace Micro.DDD.ReportingService.ViewModels
{
    public class LocationInfo
    {
        public int Id { get; set; }
        public string VillageName { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}