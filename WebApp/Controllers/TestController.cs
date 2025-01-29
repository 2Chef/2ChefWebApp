using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{

    [ApiController]
    [Route("/")]
    internal class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("TestController is working!");
        }
    }
}
