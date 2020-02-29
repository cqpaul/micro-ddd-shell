/**
*@Project: Micro.DDD.Common
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 10:03:41 AM
*/

namespace Micro.DDD.Common.Consul
{
    public class ConsulOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServiceIp { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }

        /// <summary>
        /// Consul 地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}