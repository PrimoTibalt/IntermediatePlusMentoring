using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repositories
{
	sealed class PriceRepository : IGenericRepository<Price>
	{
		private readonly EventContext _context;

		public PriceRepository(EventContext context)
		{
			_context = context;
		}

		public void Create(Price entity)
		{
			_context.Prices.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			_context.Prices.Remove(entity);
		}

		public async Task<IList<Price>> GetAll()
		{
			return await _context.Prices.ToListAsync();
		}

		public async Task<Price> GetById(int id)
		{
			return await _context.Prices.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Price entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
