using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData([
            new ApplicationUser {
                Id = "402613fb-372f-49e3-a66e-f76d21c9e392",
                Email = "test@test.com",
                NormalizedEmail = "test@test.com",
                Name = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                NormalizedUserName = "Admin",
                PasswordHash = hasher.HashPassword(null, "123456"),
                EmailConfirmed = true,
            }
        ]);
    }
}
