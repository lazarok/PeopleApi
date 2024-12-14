using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace People.Application.Repositories.Common;

public interface IRepository<TEntity> where TEntity : class
{
    public DbContext Context { get; }
    TEntity? GetById(long id);
    Task<TEntity?> GetByIdAsync(long id, string[]? eagerIncludes = null, CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    void Add(TEntity entity);
    bool Any(Expression<Func<TEntity, bool>> expression);
    bool Any();
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(params TEntity[] entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);


    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        string[]? eagerIncludes = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void SaveChanges(CancellationToken cancellationToken = default);
}