/**
*@Project: Micro.DDD.MessagingService
*@author: Paul Zhang
*@Date: Monday, January 6, 2020 9:21:20 AM
*/

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Micro.DDD.MessingService.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}