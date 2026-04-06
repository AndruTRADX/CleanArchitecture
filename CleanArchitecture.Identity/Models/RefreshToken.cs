using System;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Identity.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Identity.Models;

public class RefreshToken : BaseDomainModel
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public bool IsBeingUsed { get; set; }
    public bool HasBeenRevoked { get; set; }
    public DateTime ExpireDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual IdentityUser? User { get; set; }
}
