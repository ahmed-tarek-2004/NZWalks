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
      //  private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly IMapper map;
        public ImageController(IImageServices imageServices,
            IWebHostEnvironment webHostEnvironment)
        {
            this.imageServices = imageServices;
            //this.map = map;
            this.webHostEnvironment = webHostEnvironment;
           // this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Upload")]
        [ImageValidation]
        public async Task<IActionResult> Upload([FromForm] ImageFileDTO request)
        {
            var FullPath= $"{Request.Scheme}://{Request.Host}" +
                $"{Request.PathBase}";
           
            var image= await imageServices.Upload(request,webHostEnvironment.ContentRootPath,
                FullPath);

            return Ok(image);
        }
    }
}
