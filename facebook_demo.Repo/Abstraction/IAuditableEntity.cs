namespace facebook_demo.Repo.Abstraction;

public interface IAuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; } 
    public DateTimeOffset? UpdatedAt { get; set; } 
}