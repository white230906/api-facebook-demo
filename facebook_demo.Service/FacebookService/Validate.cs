using facebook_demo.Service.Exceptions;

namespace facebook_demo.Service.FacebookService;

public static class Validate
{
    public static void FacebookConfiguration(FacebookOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.PageId) ||
            string.IsNullOrWhiteSpace(options.AccessToken) ||
            string.IsNullOrWhiteSpace(options.TokenType))
        {
            throw new FacebookConfigurationException();
        }
    }

    public static void CreatePostRequest(CreateFacebookPostRequest request)
    {
        if (request is null)
        {
            throw new FacebookRequestException("Request body is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            throw new FacebookRequestException("Post message is required.");
        }

        if (request.ImageUrls is null || request.ImageUrls.Count == 0)
        {
            throw new FacebookRequestException("At least one image URL is required.");
        }

        if (request.ImageUrls.Count > 10)
        {
            throw new FacebookRequestException("A post can contain at most 10 images.");
        }

        foreach (var imageUrl in request.ImageUrls)
        {
            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
            {
                throw new FacebookRequestException($"Invalid image URL: {imageUrl}");
            }
        }
    }
}
