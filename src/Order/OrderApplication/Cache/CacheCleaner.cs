using API.Abstraction.Cache;
using DAL.Events;
using StackExchange.Redis;

namespace OrderApplication.Cache
{
	internal class CacheCleaner : ICacheCleaner
	{
		private readonly IDatabase _database;

		public CacheCleaner(IDatabase database)
		{
			_database = database;
		}

		public async Task CleanEventSeatsCache(IList<EventSeat> seats)
		{
			if (seats.Any(s => s?.Seat?.Row is null))
				return;

			var pairs = new HashSet<EventIdSectionId>();
			foreach (var seat in seats)
			{
				if (pairs.Add(new(seat.EventId, seat.Seat.Row.SectionId)))
				{
					await _database.ExecuteAsync("DEL", string.Format(EventCacheKeysTemplates.EventAppEventSeatsByEventIdSectionIdCacheTemplate, seat.EventId, seat.Seat.Row.SectionId));
				}
			}
		}

		private record EventIdSectionId(int eventId, int sectionId);
	}
}
