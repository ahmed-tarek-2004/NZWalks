using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using NZWalks.Validation;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageServices imageServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<ImageController> logger;
        //private readonly IMapper map;
        public ImageController(IImageServices imageServices,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ImageController> logger)
        {
            this.imageServices = imageServices;
            //this.map = map;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            // this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Upload")]
        [ServiceFilter(typeof(ImageValidation))]
        public async Task<IActionResult> Upload([FromForm] ImageFileDTO request)
        {
            var FullPath= $"{Request.Scheme}://{Request.Host}" +
                $"{Request.PathBase}";
           
            var image= await imageServices.Upload(request,webHostEnvironment.ContentRootPath,
                FullPath);
            logger.LogDebug("Works Successful");
            return Ok(image);
        }
    }
}
