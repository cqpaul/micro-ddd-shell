/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Friday, December 20, 2019 10:46:35 AM
*/

using System;
using System.Collections.Generic;
using Micro.DDD.ReportingService.ViewModels;

namespace Micro.DDD.ReportingService.Interfaces
{
    public interface IReportingService: IDisposable
    {
        List<string> GetCurrentAllVillages(string queryString);
        List<VillageBaseInfoViewModel> GetAllVillageBaseInfos(string villageName);

        List<SummaryItemViewModel> GetSummaryInfo();

        IEnumerable<KeyValuePair<string,int>> GetWordCountDictionary(string villageName);

        IEnumerable<LocationInfo> GetLocationInfos(string villageName);
    }
}