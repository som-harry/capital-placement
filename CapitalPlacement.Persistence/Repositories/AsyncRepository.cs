

namespace CapitalReplacement.Persistence.Repositories
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        protected DataContext _context;

        public AsyncRepository(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> GetById<TId>(TId id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _context.Set<TEntity>().Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.Set<TEntity>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity).State = EntityState.Deleted;
        }
        public virtual async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public virtual async Task<IQueryable<TEntity>> WhereQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsNoTracking();
        }
        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }


    }
}
