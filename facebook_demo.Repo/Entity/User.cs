using facebook_demo.Repo.Abstraction;

namespace facebook_demo.Repo.Entity;

public class User: BaseEntity<Guid>, IAuditableEntity
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}