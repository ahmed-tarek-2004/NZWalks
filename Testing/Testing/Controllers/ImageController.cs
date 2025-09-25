using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IimageUploadService _cloudinaryService;
        private static Dictionary<string, string> FilesDb = new();
        public ImageController(IimageUploadService imageUploadService)
        {
            _cloudinaryService = imageUploadService;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] IFormFile file)
        {
            if (file == null) return BadRequest("No file provided Ya Negm");

            var url = await _cloudinaryService.UploadFileAsync(file);

            var fileId = Guid.NewGuid().ToString();
            FilesDb[fileId] = url.Url;

            return Ok(new { FileId = fileId, DownloadUrl = url });
        }

        [HttpGet("download/{fileId}")]
        public IActionResult Download(string fileId)
        {
            if (!FilesDb.ContainsKey(fileId)) return NotFound();

            var url = FilesDb[fileId];
            return Redirect(url); // العميل يبدأ التحميل مباشرة
        }

        [HttpGet("list")]
        public IActionResult ListFiles()
        {
            return Ok(FilesDb.Select(f => new { FileId = f.Key, Url = $"/api/files/download/{f.Key}" }));
        }
        //[HttpPost("file")]
        //public async Task<ActionResult> Upload(IFormFile file)
        //{
        //    var result = await _imageUploadService.UploadAsync(file);
        //    return Ok(result);
        //}
        //[HttpPut("update")]
        //public async Task<ActionResult> ReUpload(IFormFile file, string publicId)
        //{
        //    var result = await _imageUploadService.ReUploadAsync(file, publicId);
        //    return Ok(result);
        //}
        //[HttpDelete("delete")]
        //public async Task<ActionResult> Delete(string publicId)
        //{
        //    var result = await _imageUploadService.RemoveAsync(publicId);
        //    return Ok(result);
        //}
        //[HttpGet("get")]
        //public async Task<ActionResult> GetById([FromQuery]string Id)
        //{
        //    return Ok(await _imageUploadService.GetByIdAsync(Id));
        //}
    }
}
