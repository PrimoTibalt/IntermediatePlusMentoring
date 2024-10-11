using DAL.Venues;

namespace DAL.Events.Repository
{
	public interface IEventRepository : IGenericRepository<Event, int>
	{
		Task<Event> GetWithVenueAndSections(int eventId);
		Task<IList<Section>> GetSections(int eventId);
	}
}
