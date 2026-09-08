using System.Text.Json.Serialization;

namespace facebook_demo.Service.FacebookService;

public class CreateFacebookPostResponse
{
    public string PostId { get; set; } = string.Empty;
    public List<string> PhotoIds { get; set; } = [];
    public string PostUrl { get; set; } = string.Empty;
}

internal class FacebookIdResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
