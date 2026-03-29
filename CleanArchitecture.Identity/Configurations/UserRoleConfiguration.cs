

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData([
            new IdentityUserRole<string> {
                RoleId = "36ae9a08-8fbe-4abc-aea2-83d189670356",
                UserId = "402613fb-372f-49e3-a66e-f76d21c9e392"
            }
        ]);
    }
}
