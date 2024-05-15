
namespace CapitalReplacement.Application.Interfaces.Persistence;
public interface IAsyncRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetById<TId>(TId id);
    Task<IEnumerable<TEntity>> GetAll(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate = null);
    Task<IEnumerable<TEntity>> GetAll(int pageNumber, int pageSize);
    Task<TEntity> Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> WhereQueryable(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
}

