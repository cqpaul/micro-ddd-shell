/**
*@Project: Micro.DDD.ReportingService
*@author: Paul Zhang
*@Date: Wednesday, December 18, 2019 10:16:43 AM
*/

using Microsoft.AspNetCore.Mvc;

namespace Micro.DDD.ReportingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet("healthCheck")]
        public IActionResult Check() => Ok("OK");
    }
}