/**
*@Project: Micro.DDD.ETLWorker
*@author: Paul Zhang
*@Date: Sunday, December 29, 2019 3:13:10 PM
*/

using System;

namespace Micro.DDD.ETLWorker.BaiduObject
{
    public class RootObject
    {
        public int Status { get; set; }
        public ResultObj Result { get; set; }
    }

    public class ResultObj
    {
        public Location Location { get; set; }
        public int Precise { get; set; }
        public int Confidence { get; set; }
        public int Comprehension { get; set; }
        public string Level { get; set; }
    }

    public class Location
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}