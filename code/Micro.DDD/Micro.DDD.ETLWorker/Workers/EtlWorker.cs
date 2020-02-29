/**
*@Project: Micro.DDD.ETLWorker
*@author: Paul Zhang
*@Date: Thursday, December 19, 2019 11:16:34 AM
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Micro.DDD.ETLWorker.BaiduObject;
using Micro.DDD.ETLWorker.Models;
using Micro.DDD.ETLWorker.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Micro.DDD.ETLWorker.Workers
{
    public class EtlWorker
    {
        private readonly MongoDbUtil _mongoDbUtil;
        private readonly string _mysqlConStr;

        public EtlWorker(MongoDbUtil mongoDbUtil, string mysqlConStr)
        {
            _mongoDbUtil = mongoDbUtil;
            this._mysqlConStr = mysqlConStr;
        }

        // Data From MongoDB to Mysql
        public void EtlWorkerTask(string villageName)
        {
            // 1. Data from MongoDB
            List<ShellNode> villageNodes = _mongoDbUtil.GetMongoCollectionData(villageName);
            // 2. Data to Mysql
            if (villageNodes.Any())
            {
                MySqlUtil mySqlUtil = new MySqlUtil(_mysqlConStr);
                mySqlUtil.BulkInsertNodes(villageName, villageNodes);
                UpdateAllLocationInfo();
                mySqlUtil.Dispose();
            }
            Console.WriteLine($"{DateTime.Now}: <{villageName}> Finished.");
        }

        public void UpdateAllLocationInfo()
        {
            MySqlUtil mySqlUtil = new MySqlUtil(_mysqlConStr);
            List<string> needUpdateVillages = mySqlUtil.GetVillagesWithoutLocation();
            List<LocationInfo> locationInfos = new List<LocationInfo>();
            foreach (string village in needUpdateVillages)
            {
                LocationInfo locationInfo = GetNewVillageLocation(village);
                if (locationInfo != null && locationInfo.VillageName != null)
                {
                    locationInfos.Add(locationInfo);
                }
            }
            mySqlUtil.InsertLocationInfos(locationInfos);
        }

        // Get Village location: lat & lag from Baidu
        // http://lbsyun.baidu.com/index.php?title=webapi/guide/webservice-geocoding
        private LocationInfo GetNewVillageLocation(string villageName)
        {
            LocationInfo locationInfo = new LocationInfo();
            try
            {
                string baiAk = "7UA0GQRgTDFVcQ8bnpA7LFsFiFQRIphS";
                string getUrl = $"http://api.map.baidu.com/geocoding/v3/?address=重庆市{villageName}&output=json&ak={baiAk}";
                string response = HttpRequestUtil.CreateGetHttpResponse(getUrl);
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(response);
                // Update to the database
                if (rootObject.Status == 0)
                {
                    locationInfo.VillageName = villageName;
                    locationInfo.Lat = rootObject.Result.Location.Lat;
                    locationInfo.Lng = rootObject.Result.Location.Lng;
                }
                Console.WriteLine(response);
                Console.WriteLine($"{DateTime.Now}: get location info from BaiDu Map for {villageName}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return locationInfo;
        }
    }
}