/**
*@Project: Micro.DDD.ETLWorker
*@author: Paul Zhang
*@Date: Monday, January 6, 2020 1:59:43 PM
*/

using WebApiClient;
using WebApiClient.Attributes;

namespace Micro.DDD.ETLWorker.HttpAPI
{
    [TraceFilter]
    public interface IMessageClient: IHttpApi
    {
        [HttpGet("/api/Message/FinishedEtl")]
        ITask<string> TriggerRefreshEvent(string message);
    }
}