using facebook_demo.Service.FacebookService;
using facebook_demo.Service.Models;
using Microsoft.AspNetCore.Mvc;
using FacebookService = facebook_demo.Service.FacebookService;

namespace facebook_demo.API.Controller;

[ApiController]
[Route("api/facebook")]
public class FacebookController : ControllerBase
{
    private readonly FacebookService.IService _facebookService;

    public FacebookController(FacebookService.IService facebookService)
    {
        _facebookService = facebookService;
    }

    [HttpPost("posts")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<BaseResponse>> CreatePost(
        [FromBody] CreateFacebookPostRequest request)
    {
        var result = await _facebookService.CreatePostAsync(request);
        return Ok(ApiResponseFactory.Base(result));
    }
}
