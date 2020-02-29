/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Friday, December 20, 2019 10:47:49 AM
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.Common;
using Micro.DDD.ReportingService.Interfaces;
using Micro.DDD.ReportingService.ViewModels;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Micro.DDD.ReportingService.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IDbConnection _dbConnection;

        public ReportingService(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration["mysql_connection_string"]);
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
            GC.SuppressFinalize(this);
        }

        public List<string> GetCurrentAllVillages(string queryString)
        {
            List<string> villages = new List<string>();
            villages.Add("1号线");
            villages.Add("2号线");
            villages.Add("3号线");
            villages.Add("4号线");
            villages.Add("5号线");
            villages.Add("6号线");
            villages.Add("10号线");
            villages.Add("环线");
            villages.Add("国博线");
            var result = _dbConnection.Query<string>("select distinct(Position) from ShellDatas where POSITION like CONCAT('%',@queryString,'%')",
                new {queryString = queryString});
            if (result.Any())
            {
                villages.AddRange(result.Take(10).ToList());
            }
            return villages;
        }

        public List<VillageBaseInfoViewModel> GetAllVillageBaseInfos(string villageName)
        {
            List<VillageBaseInfoViewModel> aggregateData = new List<VillageBaseInfoViewModel>();

            List<ShellNodeViewModel> allNodes = new List<ShellNodeViewModel>();
            if (Constant.Constants.CqSubwayList.Contains(villageName))
            {
                allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas where VillageName = @VillageName", new {VillageName = villageName}).ToList();
            }
            else if (villageName != null)
            {
                allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas where Position = @VillageName", new {VillageName = villageName}).ToList();
            }
            else
            {
                allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas").ToList();
            }

            List<string> allVillages = allNodes.Select(a => a.Position).Distinct().ToList();
            foreach (string village in allVillages)
            {
                VillageBaseInfoViewModel unitVillage = new VillageBaseInfoViewModel();
                var villageNodes = allNodes.Where(a => a.Position == village).ToList();
                unitVillage.VillageName = village;
                unitVillage.SellCount = villageNodes.Count;
                var years = villageNodes.GroupBy(a => a.YearInfo).OrderByDescending(a => a.Key);
                if (years.Any())
                {
                    var yearStr = years.First().Key;
                    if (yearStr != null && yearStr.Contains("年建"))
                    {
                        unitVillage.BuildingYears = Convert.ToInt32(yearStr.Substring(0, yearStr.Length - 2));
                    }
                }

                List<double> totalPrices = new List<double>();
                List<double> totalUnitPrices = new List<double>();
                foreach (ShellNodeViewModel node in villageNodes)
                {
                    if (node.Price != null && node.Price.Contains("万"))
                    {
                        var priceStr = node.Price.Substring(0, node.Price.Length - 1);
                        double price = Convert.ToDouble(priceStr);
                        totalPrices.Add(price);
                    }

                    if (node.UnitPrice != null && node.UnitPrice.Contains("平米"))
                    {
                        string unitPriceStr = System.Text.RegularExpressions.Regex.Replace(node.UnitPrice, @"[^0-9,.]+", "");
                        double unitPrice = Convert.ToDouble(unitPriceStr);
                        totalUnitPrices.Add(unitPrice);
                    }
                }

                if (totalPrices.Any() && totalUnitPrices.Any())
                {
                    unitVillage.TopPrice = totalPrices.Max();
                    unitVillage.LowestPrice = totalPrices.Min();
                    unitVillage.AveragePrice = Math.Round(totalUnitPrices.Average(), 2);
                    unitVillage.AverageTotalPrice = Math.Round(totalPrices.Average(), 2);
                    unitVillage.TopNode = villageNodes.Where(a => a.Price.Contains(unitVillage.TopPrice.ToString())).FirstOrDefault();
                    unitVillage.LowestNode = villageNodes.Where(a => a.Price.Contains(unitVillage.LowestPrice.ToString())).FirstOrDefault();
                    aggregateData.Add(unitVillage);
                }
            }

            return aggregateData.OrderByDescending(a => a.SellCount).ToList();
        }

        public List<SummaryItemViewModel> GetSummaryInfo()
        {
            List<ShellNodeViewModel> allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas").ToList();
            int totalCount = allNodes.Count;
            int totalVillages = allNodes.Select(a => a.Position).Distinct().Count();
            List<double> totalPrices = new List<double>();
            List<double> totalUnitPrices = new List<double>();
            foreach (ShellNodeViewModel node in allNodes)
            {
                if (node.Price != null && node.Price.Contains("万"))
                {
                    var priceStr = node.Price.Substring(0, node.Price.Length - 1);
                    double price = Convert.ToDouble(priceStr);
                    totalPrices.Add(price);
                }

                if (node.UnitPrice != null && node.UnitPrice.Contains("平米"))
                {
                    string unitPriceStr = System.Text.RegularExpressions.Regex.Replace(node.UnitPrice, @"[^0-9,.]+", "");
                    double unitPrice = Convert.ToDouble(unitPriceStr);
                    totalUnitPrices.Add(unitPrice);
                }
            }

            List<SummaryItemViewModel> summaries = new List<SummaryItemViewModel>();
            SummaryItemViewModel model1 = new SummaryItemViewModel() {Title = "总收录二手房", Count = $"{totalCount} 套"};
            SummaryItemViewModel model0 = new SummaryItemViewModel() {Title = "总收录小区数", Count = $"{totalVillages} 个"};
            SummaryItemViewModel model2 = new SummaryItemViewModel() {Title = "收录最高总价", Count = $"{totalPrices.Max()} 万"};
            SummaryItemViewModel model3 = new SummaryItemViewModel() {Title = "收录最低总价", Count = $"{totalPrices.Min()} 万"};
            SummaryItemViewModel model4 = new SummaryItemViewModel() {Title = "收录平均总价", Count = $"{Math.Round(totalPrices.Average(), 2)} 万"};
            SummaryItemViewModel model5 = new SummaryItemViewModel() {Title = "收录平均单价", Count = $"{Math.Round(totalUnitPrices.Average(), 2)} 元"};
            summaries.Add(model1);
            summaries.Add(model0);
            summaries.Add(model2);
            summaries.Add(model3);
            summaries.Add(model4);
            summaries.Add(model5);
            return summaries;
        }

        public IEnumerable<KeyValuePair<string,int>> GetWordCountDictionary(string villageName)
        {
            List<ShellNodeViewModel> allNodes = new List<ShellNodeViewModel>();
            if (villageName == null)
            {
                allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas").ToList();
            }
            else
            {
                if (Constant.Constants.CqSubwayList.Contains(villageName))
                {
                    allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas where VillageName = @VillageName", new {VillageName = villageName}).ToList();
                }
                else
                {
                    allNodes = _dbConnection.Query<ShellNodeViewModel>("select * from ShellDatas where Position = @VillageName", new {VillageName = villageName}).ToList();
                }
            }
            List<string> allTitles = allNodes.Select(a => a.Title).ToList();
            string titleStr = string.Join(',', allTitles.ToArray());
            var seg = new JiebaSegmenter();
            var freqs = new Counter<string>(seg.Cut(titleStr));
            var result = freqs.MostCommon(200);
            return result;
        }

        public IEnumerable<LocationInfo> GetLocationInfos(string villageName)
        {
            List<LocationInfo> allLocations = new List<LocationInfo>();
            if (villageName == null)
            { 
                allLocations = _dbConnection.Query<LocationInfo>("select * from VillageLocation").ToList();
            }
            else
            {
                if (Constant.Constants.CqSubwayList.Contains(villageName))
                {
                    List<string> allVillageNames = _dbConnection
                        .Query<string>("select Position from ShellDatas where VillageName = @VillageName", new {VillageName = villageName}).ToList();
                    allLocations = _dbConnection.Query<LocationInfo>("select * from VillageLocation where VillageName in @AllVillageNames", new {AllVillageNames = allVillageNames}).ToList();
                }
                else
                {
                    allLocations = _dbConnection.Query<LocationInfo>("select * from VillageLocation where VillageName = @VillageName", new {VillageName = villageName}).ToList();
                }
            }
            return allLocations;
        }
    }
   }