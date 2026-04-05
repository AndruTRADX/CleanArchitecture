using CleanArchitecture.Application.Specifications;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Specification;

public class SpecificationEvaluator<T> where T : BaseDomainModel
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderBy);
        }

        if (specification.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        inputQuery = specification.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

        return inputQuery;
    }
}
