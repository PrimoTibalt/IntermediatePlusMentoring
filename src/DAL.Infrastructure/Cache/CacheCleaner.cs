using DAL.Events;
using DAL.Infrastructure.Cache;
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

			var keys = new HashSet<string>();
			foreach (var seat in seats)
			{
				keys.Add(string.Format(EventCacheKeysTemplates.EventAppEventSeatsByEventIdSectionIdCacheTemplate,
					seat.EventId,
					seat.Seat.Row.SectionId));
			}

			await _database.KeyDeleteAsync(keys.Select(k => new RedisKey(k)).ToArray());
		}
	}
}
