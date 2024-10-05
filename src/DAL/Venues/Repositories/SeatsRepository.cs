using Microsoft.EntityFrameworkCore;

namespace DAL.Venues.Repositories
{
	sealed class SeatsRepository : IGenericRepository<Seat, int>
	{
		private readonly VenueContext _context;

		public SeatsRepository(VenueContext context)
		{
			_context = context;
		}

		public void Create(Seat entity)
		{
			_context.Seats.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);

			_context.Seats.Remove(entity);
		}

		public async Task<IList<Seat>> GetAll()
		{
			return await _context.Seats.ToListAsync();
		}

		public async Task<Seat> GetById(int id)
		{
			return await _context.Seats.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Seat entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
