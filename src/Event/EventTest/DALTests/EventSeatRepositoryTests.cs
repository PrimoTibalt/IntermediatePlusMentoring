using DAL;
using DAL.Events;
using DAL.Events.Repository;
using Microsoft.Extensions.DependencyInjection;
using TestsCore;

namespace EventTest.DALTests
{
	public class EventSeatRepositoryTests
	{
		[Fact]
		public async Task GetEventSectionSeats_ExistingValues_EmptyIfInputIncorrect()
		{
			var serviceProvider = ServiceConfigurationProvider.Get<EventContext>(services => services.AddEventsRepositories());
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var incorrectEventId = 3453;
			var seatCount = 100;
			foreach (var index in Enumerable.Range(1, seatCount))
			{
				if (index == incorrectEventId) continue;
				await repository.Create(new EventSeat { EventId = index, Seat = new() { Row = new() { SectionId = index } } });
			}
			await repository.Save();

			List<IList<EventSeat>> results = [];
			foreach (var index in Enumerable.Range(1, seatCount))
			{
				results.Add(await repository.GetEventSectionSeats(incorrectEventId, index));
			}

			Assert.Equal(seatCount, results.Count);
			Assert.All(results, Assert.Empty);
		}

		[Fact]
		public async Task GetEventSectionSeats_ExistingValues_AllRelatedSeats()
		{
			var serviceProvider = ServiceConfigurationProvider.Get<EventContext>(services => services.AddEventsRepositories());
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var seatCount = 100;
			List<Tuple<long, long, long>> seatsIds = new();
			foreach (var index in Enumerable.Range(1, seatCount))
			{
				var seat1 = new EventSeat { EventId = index, Price = new(), Seat = new() { Row = new() { SectionId = 2 * index } } };
				var seat2 = new EventSeat { EventId = index, Price = new(), Seat = new() { Row = new() { SectionId = 2 * index } } };
				var seat3 = new EventSeat { EventId = index, Price = new(), Seat = new() { Row = new() { SectionId = 2 * index } } };
				await repository.Create(seat1);
				await repository.Create(seat2);
				await repository.Create(seat3);
				seatsIds.Add(new(seat1.Id, seat2.Id, seat3.Id));
			}
			await repository.Save();

			List<IList<EventSeat>> results = [];
			foreach (var index in Enumerable.Range(1, seatCount))
			{
				results.Add(await repository.GetEventSectionSeats(index, 2*index));
			}

			Assert.Equal(seatCount, results.Count);
			Assert.All(results, items => Assert.Equal(3, items.Count));
			Assert.All(seatsIds, items =>
			{
				Assert.Contains(results, (list) => list.Any(i => items.Item1 == i.Id) &&
					list.Any(i => items.Item2 == i.Id) &&
					list.Any(i => items.Item3 == i.Id));
			});
		}
	}
}
