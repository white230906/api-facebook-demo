using facebook_demo.Service.Models;
using Microsoft.AspNetCore.Mvc;
using MediaService = facebook_demo.Service.MediaService;

namespace facebook_demo.API.Controller;

[ApiController]
[Route("api/cloud")]
public class CloudController : ControllerBase
{
    private readonly MediaService.IService _mediaService;

    public CloudController(MediaService.IService mediaService)
    {
        _mediaService = mediaService;
    }

    /// <summary>
    /// Uploads an image to Cloudinary and returns its public HTTPS URL.
    /// </summary>
    [HttpPost("images")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult<BaseResponse>> UploadImage(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            ModelState.AddModelError(nameof(file), "Vui lòng chọn một ảnh để tải lên.");
            return ValidationProblem(ModelState);
        }

        var imageUrl = await _mediaService.UploadImageAsync(file);

        return Ok(ApiResponseFactory.Base(new
        {
            Url = imageUrl
        }));
    }
}
