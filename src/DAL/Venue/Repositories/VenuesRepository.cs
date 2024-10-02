using Microsoft.EntityFrameworkCore;

namespace DAL.Venue.Repositories
{
	sealed class VenuesRepository : IGenericRepository<Venue, int>
	{
		private readonly VenueContext _context;

		public VenuesRepository(VenueContext context)
		{
			_context = context;
		}

		public void Create(Venue entity)
		{
			_context.Venues.Add(entity);
		}

		public async Task Delete(int id)
		{
			var venue = await GetById(id);

			_context.Venues.Remove(venue);
		}

		public async Task<Venue> GetById(int id)
		{
			return await _context.Venues.Include(v => v.Sections).FirstOrDefaultAsync(v => v.Id == id);
		}

		public void Update(Venue entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		public async Task<IList<Venue>> GetAll()
		{
			return await _context.Venues.ToListAsync();
		}

		public Task<int> Save()
		{
			return _context.SaveChangesAsync();
		}
	}
}
