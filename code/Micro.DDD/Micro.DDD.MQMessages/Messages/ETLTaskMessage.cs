/**
*@Project: Micro.DDD.MQMessages
*@author: Paul Zhang
*@Date: Thursday, December 19, 2019 10:58:51 AM
*/

namespace Micro.DDD.Messages.Messages
{
    public class ETLTaskMessage
    {
        public bool ETLStartTask { get; set; }
        public string VillageName { get; set; }
    }
}