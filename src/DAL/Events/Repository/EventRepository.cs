using DAL.Venues;
using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repository
{
	internal sealed class EventRepository : GenericRepository<Event, int, EventContext>, IEventRepository
	{
		public EventRepository(EventContext context) : base(context) { }

		public async Task<IList<Section>> GetSections(int eventId)
		{
			var item = await GetWithVenueAndSections(eventId);
			if (item is null) return null;
			return (IList<Section>)(item.Venue?.Sections ?? []);
		}

		public async Task<Event> GetWithVenueAndSections(int eventId)
		{
			return await _collection.Include(e => e.Venue).Include(e => e.Venue.Sections).FirstOrDefaultAsync(e => e.Id == eventId);
		}
	}
}
