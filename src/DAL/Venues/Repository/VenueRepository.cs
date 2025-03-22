using DAL.Abstraction;
using Entities.Venues;

namespace DAL.Venues.Repository
{
	internal sealed class VenueRepository : GenericRepository<Venue, int, VenueContext>, IVenueRepository
	{
		public VenueRepository(VenueContext context) : base(context) { }
	}
}
