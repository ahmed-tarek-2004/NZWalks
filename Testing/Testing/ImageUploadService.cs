using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Testing
{
    public class ImageUploadService : IimageUploadService
    {
        private readonly Cloudinary cloudinary;
        public ImageUploadService(IConfiguration config)
        {
            var account = new Account(
         config["Cloudinary:CloudName"],
         config["Cloudinary:ApiKey"],
         config["Cloudinary:ApiSecret"]);
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
        }

        public async Task<CloudinaryResult> GetByIdAsync(string publicId)
        {
            var Recource= new GetResourceParams(publicId);
            var result = await cloudinary.GetResourceAsync(Recource);
            var r= new CloudinaryResult
            {
                Url = result.Url.ToString(),
                PublicId=result.PublicId
            };
            return r;
        }

        public async Task<string> RemoveAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("PublicId is required");
            var deletion = new DeletionParams(publicId)
            {
                Invalidate = true
            };

            var result = await cloudinary.DestroyAsync(deletion);
            return result.Result;
        }

        public async Task<CloudinaryResult> ReUploadAsync(IFormFile file, string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("PublicId is required");

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            var deletion = new DeletionParams(publicId)
            {
                Invalidate = true
            };
            var result = await cloudinary.DestroyAsync(deletion);
            if (result==null)
            {
                return null;
            }
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Overwrite = true,      // ensures replacement
                Invalidate = true,      // clears cached CDN versions
                PublicId = publicId
            };
            var resultupdate = await cloudinary.UploadAsync(imageUploadParams);
            return new CloudinaryResult
            {
                Url = resultupdate.Url.ToString(),
                PublicId = resultupdate.PublicId
            };
        }

        public async Task<CloudinaryResult> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            // await using var stream = file.OpenReadStream();
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var imageupload = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };
            var result = await cloudinary.UploadAsync(imageupload);
            //var result = await _cloudinary.UploadAsync(uploadParams);

            if (result == null)
                throw new Exception("Upload result was null from Cloudinary.");

            if (result.Error != null)
                throw new Exception($"Cloudinary error occurred: {result.Error.Message}");

            return new CloudinaryResult
            {
                Url = result.Url.ToString(),
                PublicId = result.PublicId
            };
        }
        public async Task<CloudinaryResult> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            // await using var stream = file.OpenReadStream();
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var imageupload = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };
            var result = await cloudinary.UploadAsync(imageupload);
            //var result = await _cloudinary.UploadAsync(uploadParams);

            if (result == null)
                throw new Exception("Upload result was null from Cloudinary.");

            if (result.Error != null)
                throw new Exception($"Cloudinary error occurred: {result.Error.Message}");

            return new CloudinaryResult
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }

    }
}
