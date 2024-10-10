using Microsoft.EntityFrameworkCore;

namespace DAL.Venues.Repository
{
	internal sealed class VenueRepository : GenericRepository<Venue, int, VenueContext>, IVenueRepository
	{
		public VenueRepository(VenueContext context) : base(context) { }

		public async Task<Venue> GetDetailed(int id)
		{
			var entity = await _collection.Include(v => v.Sections)
				.FirstOrDefaultAsync(v => v.Id == id);
			return entity;
		}
	}
}
