using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain;

public class Director : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    public string FullName => $"{Name} {LastName}"; // This is how you can create calculated fields in the domain

    public virtual ICollection<Video> Videos { get; set; } = [];
}