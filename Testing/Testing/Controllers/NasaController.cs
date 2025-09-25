using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testing.AuthServices;

namespace Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NasaController : ControllerBase
    {
        private readonly NasaService _spaceTrackService;

        public NasaController(NasaService spaceTrackService)
        {
            _spaceTrackService = spaceTrackService;
        }

        [HttpGet("satellites")]
        public async Task<IActionResult> GetSatellites()
        {
            var data = await _spaceTrackService.GetSatellitesAsync();
            return Ok(data);
        }
    }
}
