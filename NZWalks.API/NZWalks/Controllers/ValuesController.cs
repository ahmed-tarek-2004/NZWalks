using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiVersion("2.0")]
    //[ApiVersion("1.0")]
    public class ValuesController : ControllerBase
    {
       // [MapToApiVersion("1.0")]
        [HttpGet("g1")]
        public IActionResult getV1()
        {
            return Ok("Version 1");
        }

       // [MapToApiVersion("2.0")]
        [HttpGet("g2")]
        public IActionResult getV2()
        {
            return Ok("Version 2");
        }
    }
}
