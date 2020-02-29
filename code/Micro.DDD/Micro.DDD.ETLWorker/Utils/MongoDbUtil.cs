/**
*@Project: Micro.DDD.ETLWorker
*@author: Paul Zhang
*@Date: Thursday, December 19, 2019 1:10:35 PM
*/

using System.Collections.Generic;
using Micro.DDD.ETLWorker.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Micro.DDD.ETLWorker.Utils
{
    public class MongoDbUtil
    {
        private readonly MongoClient _mongoClient;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public MongoDbUtil(string mongoUrl, string collectionName, string databaseName)
        {
            this._collectionName = collectionName;
            _databaseName = databaseName;
            _mongoClient = new MongoClient(mongoUrl);
        }

        public List<ShellNode> GetMongoCollectionData(string villageName)
        {
            var database = _mongoClient.GetDatabase(_databaseName);
            var collection = database.GetCollection<BsonDocument>(_collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("VillageName", villageName);
            var result = collection.Find(filter).As<ShellNode>().ToList();
            return result;
        }
    }
}