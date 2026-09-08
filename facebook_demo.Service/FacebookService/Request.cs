using System.ComponentModel.DataAnnotations;

namespace facebook_demo.Service.FacebookService;

public class CreateFacebookPostRequest
{
    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public List<string> ImageUrls { get; set; } = [];
}
