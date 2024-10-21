﻿using DAL;
using DAL.Events;
using DAL.Orders;
using DAL.Orders.Repository;
using Microsoft.Extensions.DependencyInjection;
using TestsCore;

namespace OrderTests.DAL
{
	public class EventSeatRepositoryTests
	{
		[Fact]
		public async Task GetBy_ExistingValues_ReturnsCorrect()
		{
			var serviceProvider = ServiceConfigurationProvider.Get<OrderContext>(services => services.AddOrderRepositories());
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var seatsCount = 100;
			var eventIdSeatId = await FillEventSeats(repository, seatsCount);

			var result = new List<EventSeat>();
			foreach (var pair in eventIdSeatId)
			{
				var item = await repository.GetBy(pair.EventId, pair.SeatId);
				result.Add(item);
			}

			Assert.All(eventIdSeatId, (pair) =>
			{
				Assert.Single(result, item => pair.EventId == item.EventId && pair.SeatId == item.Id);
			});
		}

		[Fact]
		public async Task GetBy_ExistingValues_ReturnsNullOnGetWithIncorrectId()
		{
			var serviceProvider = ServiceConfigurationProvider.Get<OrderContext>(services => services.AddOrderRepositories());
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var seatsCount = 100;
			var eventIdSeatId = await FillEventSeats(repository, seatsCount);
			List<(int EventId, long SeatId)> nonExistingPairs = new();
			foreach (var id in Enumerable.Range(seatsCount, 2*seatsCount))
			{
				nonExistingPairs.Add(new(id, id));
			}

			var results = new List<EventSeat>();
			foreach (var pair in nonExistingPairs)
			{
				results.Add(await repository.GetBy(pair.EventId, pair.SeatId));
			}

			Assert.All(results, item =>
			{
				Assert.Null(item);
			});
		}

		private static async Task<IList<(int EventId, long SeatId)>> FillEventSeats(IEventSeatRepository repository, int seatsCount)
		{
			List<(int EventId, long SeatId)> result = [];
			for (var i = 0; i < seatsCount; i++)
			{
				var seat = new EventSeat
				{
					SeatId = i,
					EventId = i % (seatsCount / 3),
				};
				await repository.Create(seat);
				result.Add(new(seat.EventId, seat.Id));
			}
			await repository.Save();
			return result;
		}
	}
}
