using Microsoft.AspNetCore.Http;

namespace HospitalManagementAPI.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file);
    }
}