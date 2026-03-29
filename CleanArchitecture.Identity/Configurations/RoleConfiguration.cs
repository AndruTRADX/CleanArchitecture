using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData([
            new IdentityRole {
                Id = "36ae9a08-8fbe-4abc-aea2-83d189670356",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole {
                Id = "c008811f-99d2-48ea-9ee0-e2b96ef2a64e",
                Name = "Operator",
                NormalizedName = "OPERATOR"
            },
        ]);
    }
}
