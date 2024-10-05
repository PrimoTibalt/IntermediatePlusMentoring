using Microsoft.EntityFrameworkCore;

namespace DAL
{
	internal class BaseRepository<TEntity, TKey, TContext> : IGenericRepository<TEntity, TKey>
		where TEntity : class
		where TKey : struct
		where TContext : DbContext
	{
		private readonly TContext _context;
		private readonly string _collectionName;

		public BaseRepository(TContext context, string collectionName)
		{
			_context = context;
			_collectionName = collectionName;
		}

		public void Create(TEntity entity)
		{
			_context.Add(entity);
		}

		public async Task Delete(TKey id)
		{
			var entity = await GetById(id);
			GetCollection().Remove(entity);
		}

		public async Task<IList<TEntity>> GetAll()
		{
			return await GetCollection().ToListAsync();
		}

		public async Task<TEntity> GetById(TKey id)
		{
			return await _context.FindAsync<TEntity>(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		private DbSet<TEntity> GetCollection()
		{
			var contextType = typeof(TContext);
			var propertyInfo = contextType.GetProperty(_collectionName);
			return (DbSet<TEntity>) propertyInfo.GetValue(_context);
		}
	}
}
