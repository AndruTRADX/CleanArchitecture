using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Directors;

public class DirectorCountSpecification(DirectorSpecificationParams specification) : BaseSpecification<Director>(
    x => 
        string.IsNullOrWhiteSpace(specification.Search) || x.Name.Contains(specification.Search)
    )
{
}
