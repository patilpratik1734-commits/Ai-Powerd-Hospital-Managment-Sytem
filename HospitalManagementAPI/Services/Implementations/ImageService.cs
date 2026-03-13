
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HospitalManagementAPI.Services.Interfaces;

namespace HospitalManagementAPI.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService()
        {
            var account = new Account(
                "Hospital-Management",
                "421165848872419",
                "zay5zOC4lroAnirdfkjanvmmOBk"
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }
    }
}