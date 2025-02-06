using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.SharedContext.Data;

public interface IRepository<TEntity> where TEntity : Entity
{
    void Delete(TEntity entity);

    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params string[] includes);

    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);

    void Insert(TEntity entity);

    void Update(TEntity entity);
}

