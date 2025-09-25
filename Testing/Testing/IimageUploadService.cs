namespace Testing
{
    public interface IimageUploadService
    {
        public Task<CloudinaryResult> UploadAsync(IFormFile file);
        public Task<CloudinaryResult> ReUploadAsync(IFormFile file,string publidid);
        public Task<string> RemoveAsync(string publicid);
        public Task<CloudinaryResult> GetByIdAsync(string publicId);
        public Task<CloudinaryResult> UploadFileAsync(IFormFile file);
    }
}
