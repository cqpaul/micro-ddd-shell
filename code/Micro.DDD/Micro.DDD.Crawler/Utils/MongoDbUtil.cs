/**
*@Project: Micro.DDD.Crawler
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 9:49:54 AM
*/

using System.Collections.Generic;
using System.Linq;
using Micro.DDD.Crawler.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Micro.DDD.Crawler.Utils
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

        private BsonDocument ShellModeToBsonDocument(ShellNode shellNode)
        {
            BsonDocument document = BsonDocument.Parse(shellNode.ToJson());
            return document;
        }

        public void InsertShellNodes(List<ShellNode> shellNodes)
        {
            if(!shellNodes.Any()) return;
            var collection = _mongoClient.GetDatabase(_databaseName).GetCollection<BsonDocument>(_collectionName);
            List<BsonDocument> documents = new List<BsonDocument>();
            foreach (ShellNode shellNode in shellNodes)
            {
                BsonDocument document = ShellModeToBsonDocument(shellNode);
                documents.Add(document);
            }
            collection.InsertMany(documents);
        }
    }
}