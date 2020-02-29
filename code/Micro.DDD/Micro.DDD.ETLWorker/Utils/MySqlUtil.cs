/**
*@Project: Micro.DDD.ETLWorker
*@author: Paul Zhang
*@Date: Thursday, December 19, 2019 1:37:38 PM
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Micro.DDD.ETLWorker.Models;
using MySql.Data.MySqlClient;

namespace Micro.DDD.ETLWorker.Utils
{
    public class MySqlUtil : IDisposable
    {
        private readonly IDbConnection _dbCon;

        public MySqlUtil(string connectionStr)
        {
            _dbCon = new MySqlConnection(connectionStr);
        }

        public IEnumerable<ShellNode> GetAllVillageData(string villageName)
        {
            var result = _dbCon.Query<ShellNode>("select * from ShellDatas where VillageName=@VillageName", new {VillageName = villageName});
            return result;
        }
        
        // Get Villages with out location info
        public List<string> GetVillagesWithoutLocation()
        {
            var allVillages = _dbCon.Query<string>("SELECT distinct(Position) FROM ShellDatas");
            var existVillages = _dbCon.Query<string>("SELECT distinct(VillageName) FROM VillageLocation");
            List<string> needUpdateVillages = allVillages.Except(existVillages).ToList();
            return needUpdateVillages;
        }

        // Insert new location info list
        public int InsertLocationInfos(List<LocationInfo> locationInfos)
        {
            int insertCount = 0;
            if (locationInfos.Any())
            {
                insertCount = _dbCon.Execute("Insert into VillageLocation(VillageName, Lat, Lng) values (@VillageName, @Lat, @Lng)", locationInfos);
            }

            return insertCount;
        }

        public int BulkInsertNodes(string villageName, List<ShellNode> nodes)
        {
            int insertCount = 0;
            var oldData = GetAllVillageData(villageName);
            if (!oldData.Any())
            {
                insertCount = _dbCon.Execute("Insert into ShellDatas(VillageName, Position, Title, Floor, YearInfo, AreaStr, AreaNumber, Orientation, FollowNumber, FollowDay, Price, UnitPrice, CrawlDate, LinkUrl) values (@VillageName, @Position, @Title, @Floor, @YearInfo, @AreaStr, @AreaNumber, @Orientation, @FollowNumber, @FollowDay, @Price, @UnitPrice, @CrawlDate, @LinkUrl)", nodes);
            }
            return insertCount;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}