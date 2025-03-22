using DAL.Abstraction;
using Entities.Venues;
using Microsoft.EntityFrameworkCore;

namespace DAL.Venues.Repository
{
	internal sealed class SectionRepository : GenericRepository<Section, int, VenueContext>, ISectionRepository
	{
		public SectionRepository(VenueContext context) : base(context) { }

		public async Task<IList<Section>> GetByVenueId(int venueId, CancellationToken token = default)
		{
			return await _collection.Include(s => s.Rows).Where(s => s.VenueId == venueId).ToListAsync(token);
		}
	}
}
