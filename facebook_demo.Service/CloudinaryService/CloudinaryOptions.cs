using System.ComponentModel.DataAnnotations;

namespace facebook_demo.Service.CloudinaryService;

public class CloudinaryOptions
{
    [Required] public string CloudName { get; set; }
    [Required] public string ApiKey { get; set; }
    [Required] public string ApiSecret { get; set; }
}