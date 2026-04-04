using System.Linq.Expressions;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class BaseRepository<T>(StreamerDbContext dbContext) : IAsyncRepository<T> where T : BaseDomainModel
{
    protected readonly StreamerDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeString = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (disableTracking)
            query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
            query.Include(includeString);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (disableTracking)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public void AddEntity(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void UpdateEntity(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbContext.Set<T>().Update(entity);
    }

    public void DeleteEntity(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        var response = await _dbContext.Set<T>().Where(predicate).ToListAsync();
        return response.FirstOrDefault();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeString = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (disableTracking)
            query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
            query.Include(includeString);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
        {
            var response = await orderBy(query).ToListAsync();
            return response.FirstOrDefault();
        }

        var responseQuery = await query.ToListAsync();

        return responseQuery.FirstOrDefault();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (disableTracking)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
        {
            var response = await orderBy(query).ToListAsync();
            return response.FirstOrDefault();
        }

        var responseQuery = await query.ToListAsync();

        return responseQuery.FirstOrDefault();
    }
}
