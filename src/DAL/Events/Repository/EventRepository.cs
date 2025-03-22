using Entities.Venues;
using EventApplication.Repositories;
using Microsoft.EntityFrameworkCore;
using Entities.Events;
using DAL.Abstraction;

namespace DAL.Events.Repository
{
	internal sealed class EventRepository : GenericRepository<Event, int, EventContext>, IEventRepository
	{
		public EventRepository(EventContext context) : base(context) { }

		public async Task<IList<Section>> GetSections(int eventId)
		{
			var item = await _collection.Include(e => e.Venue)
				.ThenInclude(v => v.Sections)
				.ThenInclude(s => s.Rows)
				.FirstOrDefaultAsync(e => e.Id == eventId);
			if (item is null) return null;
			return item.Venue?.Sections.ToList() ?? [];
		}

		public async Task<Event> GetWithVenueAndSections(int eventId)
		{
			return await _collection.Include(e => e.Venue).Include(e => e.Venue.Sections).FirstOrDefaultAsync(e => e.Id == eventId);
		}
	}
}
