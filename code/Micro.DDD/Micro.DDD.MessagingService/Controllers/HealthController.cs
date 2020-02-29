/**
*@Project: Micro.DDD.MessagingService
*@author: Paul Zhang
*@Date: Monday, January 6, 2020 12:49:09 PM
*/

using Microsoft.AspNetCore.Mvc;

namespace Micro.DDD.MessingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet("healthCheck")]
        public IActionResult Check() => Ok("OK");
    }
}