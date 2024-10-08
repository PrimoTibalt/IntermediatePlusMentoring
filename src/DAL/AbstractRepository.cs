using Microsoft.EntityFrameworkCore;

namespace DAL
{
	abstract class AbstractRepository<TEntity> : ISaveUpdateRepository<TEntity> where TEntity : class
	{
		protected readonly DbContext _context;

		protected AbstractRepository(DbContext dbContext)
		{
			_context = dbContext;
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
