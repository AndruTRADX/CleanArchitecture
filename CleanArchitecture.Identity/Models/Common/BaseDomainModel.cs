using System;

namespace CleanArchitecture.Identity.Models.Common;

public class BaseDomainModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}
