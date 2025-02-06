using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Infra.Data;

public class Repository<TEntity, TContext> : IRepository<TEntity> where TEntity : Entity where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbset;

    public Repository(TContext context)
        => this._dbset = context.Set<TEntity>();

    public void Delete(TEntity entity)
        => this._dbset.Remove(entity);

    public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params string[] includes)
    {
        IQueryable<TEntity> query = this._dbset;

        foreach (var include in includes)
            query = query.Include(include);

        if (orderBy != null)
            query = orderBy(query);

        return await query.Where(predicate).AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        IQueryable<TEntity> query = this._dbset;

        foreach (var include in includes)
            query = query.Include(include);

        return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public void Insert(TEntity entity) => this._dbset.Add(entity);

    public void Update(TEntity entity)
    {
        var trackedEntity = this._dbset.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (trackedEntity != null)
            this._dbset.Entry(trackedEntity).State = EntityState.Detached;

        this._dbset.Entry(entity).State = EntityState.Modified;
    }
}
