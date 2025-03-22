using DAL.Orders.Repository;
using Entities.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL.Infrastructure.Cache.Services
{
	internal sealed class EventSeatCacheService(IServiceScopeFactory _scopeFactory, ICacheRepository _cacheRepository, ILogger<EventSeatCacheService> _logger) : ICacheService<EventSeat>
	{
		public Task Clean(Guid id)
		{
			_ = Task.Run(async () =>
			{
				await using var scope = _scopeFactory.CreateAsyncScope();
				var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();
				var cartItems = await cartRepository.GetItemsWithEventSeat(id);
				try
				{
					await Clean([.. cartItems.Select(ci => ci.EventSeat)]);
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Could not clean seats cache.");
				}
			});

			return Task.CompletedTask;
		}

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
