using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using People.Application.Repositories.Common;
using People.Domain.Common;
using People.Infrastructure.Persistence.Context;

namespace People.Infrastructure.Persistence.Common;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly PeopleContext _context;

    public BaseRepository(PeopleContext context)
    {
        _context = context;
    }
    
    public DbContext Context => _context;

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
    
    public void UpdateRange(params TEntity[] entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Where(expression);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public TEntity? GetById(long id)
    {
        return _context.Set<TEntity>().Find(id);
    }
    
    public async Task<TEntity?> GetByIdAsync(long id,  string[]? eagerIncludes = null, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Set<TEntity>()
            .AsNoTracking();

        if (eagerIncludes?.Any() == true)
        {
            foreach (var eagerInclude in eagerIncludes)
            {
                baseQuery = baseQuery.Include(eagerInclude);
            }
        }
        
        cancellationToken.ThrowIfCancellationRequested();
        
        return await baseQuery.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Any(expression);
    }
    
    public bool Any()
    {
        return _context.Set<TEntity>().Any();
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }
    
    
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        string[]? eagerIncludes = null,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Set<TEntity>()
            .AsNoTracking();

        if (eagerIncludes?.Any() == true)
        {
            foreach (var eagerInclude in eagerIncludes)
            {
                baseQuery = baseQuery.Include(eagerInclude);
            }
        }
        
        cancellationToken.ThrowIfCancellationRequested();

        return baseQuery.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }
    
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().CountAsync(predicate, cancellationToken);
    }
    
    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().AnyAsync(cancellationToken);
    }
    
    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().CountAsync(cancellationToken);
    }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.SaveChangesAsync(cancellationToken);
    }
    
    public void SaveChanges(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _context.SaveChanges();
    }
}