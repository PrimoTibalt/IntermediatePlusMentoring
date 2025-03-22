using DAL.Abstraction;
using Entities.Venues;

namespace VenueApplication.Repository
{
	public interface IVenueRepository : IGenericRepository<Venue, int>
	{
	}
}
