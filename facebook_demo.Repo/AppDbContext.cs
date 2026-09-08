using Microsoft.EntityFrameworkCore;

namespace facebook_demo.Repo;

public class AppDbContext : DbContext
{
    public  AppDbContext(DbContextOptions options) : base(options)
    {}
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        //modelBuilder.SeedData();
    }
}