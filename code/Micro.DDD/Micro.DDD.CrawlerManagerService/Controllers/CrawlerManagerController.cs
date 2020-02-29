/**
*@Project: Micro.DDD.CrawlerManagerService
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 3:20:37 PM
*/

using System.Threading.Tasks;
using EasyNetQ;
using Micro.DDD.CrawlerManagerService.ViewModels;
using Micro.DDD.Messages.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Micro.DDD.CrawlerManagerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlerManagerController: ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public CrawlerManagerController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpPost("StartNewCrawlerTask")]
        public JsonResult StartNewCrawlerTask([FromBody] NewCrawlerTaskViewModel newCrawlerTaskViewModel)
        {
            NewCrawlTaskMessage message = new NewCrawlTaskMessage {CrawlVillageName = newCrawlerTaskViewModel.VillageName};
            _bus.Publish(message, _configuration["mq_crawl_topic"]);
            return new JsonResult("Success.");
        }
    }
}