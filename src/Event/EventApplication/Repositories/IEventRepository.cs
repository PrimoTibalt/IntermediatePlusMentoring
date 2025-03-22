using DAL.Abstraction;
using Entities.Events;
using Entities.Venues;

namespace EventApplication.Repositories
{
	public interface IEventRepository : IGenericRepository<Event, int>
	{
		Task<Event> GetWithVenueAndSections(int eventId);
		Task<IList<Section>> GetSections(int eventId);
	}
}
