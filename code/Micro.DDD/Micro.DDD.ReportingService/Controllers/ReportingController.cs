/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Friday, December 20, 2019 10:44:53 AM
*/

using System.Collections.Generic;
using System.Linq;
using Micro.DDD.ReportingService.Interfaces;
using Micro.DDD.ReportingService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Micro.DDD.ReportingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingController: ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("GetCurrentAllVillages")]
        public JsonResult GetCurrentAllVillages(string queryString)
        {
            var villages = _reportingService.GetCurrentAllVillages(queryString);
//            if (villages.Any())
//            {
//                villages = villages.OrderBy(s => s).ToList();
//            }
            return new JsonResult(villages);
        }

        [HttpGet("GetAllVillageBaseInfos")]
        public JsonResult GetAllVillageBaseInfos(string subwayName)
        {
            var result = _reportingService.GetAllVillageBaseInfos(subwayName);
            return new JsonResult(result);
        }

        [HttpGet("GetSummaryInfo")]
        public JsonResult GetSummaryInfo()
        {
            List<SummaryItemViewModel> summaries = _reportingService.GetSummaryInfo();
            return new JsonResult(summaries);
        }

        [HttpGet("GetWordCountDictionary")]
        public JsonResult GetWordCountDictionary(string villageName)
        {
            var result = _reportingService.GetWordCountDictionary(villageName);
            return new JsonResult(result);
        }

        [HttpGet("GetLocationInfos")]
        public JsonResult GetLocationInfos(string villageName)
        {
            var result = _reportingService.GetLocationInfos(villageName);
            return new JsonResult(result.Take(300));
        }

    }
}