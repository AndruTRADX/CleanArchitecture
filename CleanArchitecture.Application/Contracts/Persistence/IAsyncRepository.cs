using System.Linq.Expressions;
using CleanArchitecture.Application.Specifications;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : BaseDomainModel
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeString = null,
        bool disableTracking = true
    );
    Task<IReadOnlyList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = true
    );

    Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetFirstAsync(
        Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeString = null,
        bool disableTracking = true
    );
    Task<T?> GetFirstAsync(
        Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = true
    );

    void AddEntity(T entity);
    void UpdateEntity(T entity);
    void DeleteEntity(T entity);

    Task<T?> GetByIdWithSpec(ISpecification<T> specification);
    Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification);
    Task<int> CountAsync(ISpecification<T> specification);
}
