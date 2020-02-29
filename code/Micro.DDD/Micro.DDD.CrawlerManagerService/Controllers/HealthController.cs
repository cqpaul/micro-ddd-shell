/**
*@Project: Micro.DDD.CrawlerManagerService
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 10:15:42 AM
*/

using Microsoft.AspNetCore.Mvc;

namespace Micro.DDD.CrawlerManagerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet("healthCheck")]
        public IActionResult Check() => Ok("OK");
    }
}