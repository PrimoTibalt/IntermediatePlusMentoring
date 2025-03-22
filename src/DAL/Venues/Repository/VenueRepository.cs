using DAL.Abstraction;
using Entities.Venues;
using VenueApplication.Repository;

namespace DAL.Venues.Repository
{
	internal sealed class VenueRepository : GenericRepository<Venue, int, VenueContext>, IVenueRepository
	{
		public VenueRepository(VenueContext context) : base(context) { }
	}
}
