namespace facebook_demo.Service.FacebookService;

public interface IService
{
    Task<CreateFacebookPostResponse> CreatePostAsync(CreateFacebookPostRequest request);
}
