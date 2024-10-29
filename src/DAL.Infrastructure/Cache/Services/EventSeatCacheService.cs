using DAL.Events;
using OrderApplication.Cache;

namespace DAL.Infrastructure.Cache.Services
{
	internal sealed class EventSeatCacheService(ICacheRepository _cacheRepository) : ICacheService<EventSeat>
	{
		public async Task Clean(IList<EventSeat> entities)
		{
			if (entities.Any(s => s?.Seat?.Row is null))
				return;

			if (entities.Count != 0)
			{
				var keys = new HashSet<string>();
				foreach (var seat in entities)
				{
					keys.Add(GetCacheKey(seat));
				}

				await _cacheRepository.Delete([.. keys]);
			}
		}

		public string GetCacheKey(EventSeat entity)
		{
			return string.Format(EventCacheKeysTemplates.EventAppEventSeatsByEventIdSectionIdCacheTemplate,
				entity.EventId,
				entity.Seat.Row.SectionId);
		}
	}
}
