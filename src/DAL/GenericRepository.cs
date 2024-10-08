using Microsoft.EntityFrameworkCore;

namespace DAL
{
	internal class GenericRepository<TEntity, TKey, TContext> : AbstractRepository<TEntity>, IGenericRepository<TEntity, TKey>
		where TEntity : class
		where TKey : struct
		where TContext : DbContext
	{
		private readonly DbSet<TEntity> _collection;

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

		public async Task<IList<TEntity>> GetAll()
		{
			return await _collection.ToListAsync();
		}

		public async Task<TEntity> GetById(TKey id)
		{
			return await _context.FindAsync<TEntity>(id);
		}
	}
}
