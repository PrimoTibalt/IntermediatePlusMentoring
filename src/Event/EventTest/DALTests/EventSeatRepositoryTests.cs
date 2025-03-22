using DAL;
using DAL.Events;
using Entities.Events;
using EventApplication.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Providers;

namespace EventTest.DALTests
{
	public class EventSeatRepositoryTests
	{
		private static IServiceProvider serviceProvider =>
			ServiceConfigurationProvider.Get<EventContext>(services => services.AddEventsRepositories());

		[Fact]
		public async Task GetEventSectionSeats_ExistingValues_EmptyIfInputIncorrect()
		{
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var seatCount = 100;
			var incorrectEventId = seatCount+1;
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
			var repository = serviceProvider.GetService<IEventSeatRepository>();
			var eventId = 1;
			var sectionId = 2;
			var seatsCount = 5;
			await PopulateEventSeats(repository, eventId, sectionId, seatsCount);

			var result = await repository.GetEventSectionSeats(eventId, sectionId);

			Assert.NotNull(result);
			Assert.Equal(seatsCount, result.Count);
			Assert.All(result, item =>
			{
				Assert.Equal(eventId, item.EventId);
				Assert.Equal(sectionId, item.Seat.Row.SectionId);
			});
		}

		private static async Task PopulateEventSeats(IEventSeatRepository repository, int eventId, int sectionId, int seatsCount)
		{
			for (var i = 0; i < seatsCount; i++)
			{
				var seat = new EventSeat { EventId = eventId, Price = new(), Seat = new() { Row = new() { SectionId = sectionId } } };
				await repository.Create(seat);
			}

			var seatNotInOutput1 = new EventSeat { EventId = eventId+1, Price = new(), Seat = new() { Row = new() { SectionId = sectionId+1 } } };
			var seatNotInOutput2 = new EventSeat { EventId = eventId, Price = new(), Seat = new() { Row = new() { SectionId = sectionId+1 } } };
			var seatNotInOutput3 = new EventSeat { EventId = eventId+1, Price = new(), Seat = new() { Row = new() { SectionId = sectionId } } };
			await repository.Create(seatNotInOutput1);
			await repository.Create(seatNotInOutput2);
			await repository.Create(seatNotInOutput3);
			await repository.Save();
		}
	}
}
