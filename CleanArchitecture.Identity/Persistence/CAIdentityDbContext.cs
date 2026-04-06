using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Identity.Persistence;

public class CAIdentityDbContext(DbContextOptions<CAIdentityDbContext> options) : IdentityDbContext(options)
{
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        base.OnConfiguring(optionsBuilder);
    }
}
