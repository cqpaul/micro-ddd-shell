/**
*@Project: Micro.DDD.MessagingService
*@author: Paul Zhang
*@Date: Monday, January 6, 2020 10:02:53 AM
*/

using System.Threading.Tasks;
using Micro.DDD.MessingService.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Micro.DDD.MessingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController: ControllerBase
    {
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageController(IHubContext<MessageHub> messageHub)
        {
            _messageHub = messageHub;
        }

        [HttpGet("FinishedEtl")]
        public async Task<JsonResult> FinishedEtlAsync(string message)
        {
            await _messageHub.Clients.All.SendAsync("FinishedEtl", new {message = message});
            return new JsonResult("Success");
        }

    }
}