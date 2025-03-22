using DAL.Abstraction;
using Entities.Venues;

namespace VenueApplication.Repository
{
	public interface ISectionRepository : IGenericRepository<Section, int>
	{
		Task<IList<Section>> GetByVenueId(int venueId, CancellationToken token = default);
	}
}
