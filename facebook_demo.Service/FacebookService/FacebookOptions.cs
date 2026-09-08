using System.ComponentModel.DataAnnotations;

namespace facebook_demo.Service.FacebookService;

public class FacebookOptions
{
    [Required] public string PageId { get; set; } = string.Empty;
    [Required] public string AppId { get; set; } = string.Empty;
    [Required] public string AppSecret { get; set; } = string.Empty;
    [Required] public string AccessToken { get; set; } = string.Empty;
    [Required] public string TokenType { get; set; } = string.Empty;
    [Required] public int ExpiresIn { get; set; }
}
