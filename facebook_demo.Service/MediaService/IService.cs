using Microsoft.AspNetCore.Http;

namespace facebook_demo.Service.MediaService;

public interface IService
{
    public Task<string> UploadImageAsync(IFormFile file);
}