using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Actors;

public class ActorSpecification : BaseSpecification<Actor>
{
    public ActorSpecification(ActorSpecificationParams specParams) : base(
        x =>
            string.IsNullOrWhiteSpace(specParams.Search) || x.Name.Contains(specParams.Search)
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        if (!string.IsNullOrWhiteSpace(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "name":
                    AddOrderBy(p => p.Name);
                    break;
                case "nameDesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "lastName":
                    AddOrderBy(p => p.LastName);
                    break;
                case "lastNameDesc":
                    AddOrderByDescending(p => p.LastName);
                    break;
                case "createdDate":
                    AddOrderBy(p => p.CreatedDate!);
                    break;
                case "createdDateDesc":
                    AddOrderByDescending(p => p.CreatedDate!);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
}
