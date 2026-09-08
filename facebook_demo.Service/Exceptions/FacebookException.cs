namespace facebook_demo.Service.Exceptions;

public class FacebookConfigurationException : AppException
{
    public FacebookConfigurationException()
        : base(
            "Facebook Configuration Error",
            500,
            "INVALID_FACEBOOK_CONFIGURATION",
            "FacebookOptions is missing PageId, AccessToken, or TokenType.")
    {
    }
}

public class FacebookRequestException : AppException
{
    public FacebookRequestException(string detail)
        : base("Bad Request", 400, "INVALID_FACEBOOK_POST_REQUEST", detail)
    {
    }
}

public class FacebookApiException : AppException
{
    public FacebookApiException(string detail)
        : base("Facebook API Error", 502, "FACEBOOK_API_ERROR", detail)
    {
    }
}
