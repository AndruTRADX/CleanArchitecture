using CleanArchitecture.Identity.Configurations;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Identity.Persistence;

public class CAIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public CAIdentityDbContext(DbContextOptions<CAIdentityDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());

    }
}
