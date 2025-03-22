using Microsoft.EntityFrameworkCore;

namespace DAL.Abstraction
{
	public class GenericRepository<TEntity, TKey, TContext> : AbstractRepository<TEntity>, IGenericRepository<TEntity, TKey>
		where TEntity : class
		where TKey : struct
		where TContext : DbContext
	{
		protected readonly DbSet<TEntity> _collection;

		public GenericRepository(TContext context) : base(context)
		{
			_collection = context.Set<TEntity>();
		}

		public async Task Create(TEntity entity)
		{
			await _context.AddAsync(entity);
		}

		public async Task Delete(TKey id)
		{
			var entity = await GetById(id);
			_collection.Remove(entity);
		}

		public async Task<IList<TEntity>> GetAll(CancellationToken token = default)
		{
			return await _collection.ToListAsync(token);
		}

		public async Task<TEntity> GetById(TKey id)
		{
			return await _context.FindAsync<TEntity>(id);
		}
	}
}
