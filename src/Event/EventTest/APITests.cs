using API.Abstraction.Helpers;
using Entities.Events;
using EventAPI.Controllers;
using EventApplication.Entities;
using EventApplication.Queries;
using Microsoft.AspNetCore.Mvc;
using TestsCore;
using TestsCore.Providers;

namespace EventTest
{
	public class APITests
	{
		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetAll_ResultNotNull_ReturnsAllData(GetValuesSuites suite)
		{
			var events = ListGenerator.Generate<Event>(suite);
			var mediator = MediatorMockObjectBuilder.Get<GetAllEventsQuery, IList<Event>>(events);
			var cache = DistributedCacheMockObjectProvider.Get();
			var controller = ControllerProvider.GetWithCache<EventsController>(mediator, cache);

			var result = await controller.GetAll(CancellationToken.None);
			var value = (result as OkObjectResult).Value as Resource<IList<Event>>;

			Assert.NotNull(value?.Value);
			Assert.Equal(events.Count, value.Value.Count);
		}

		[Fact]
		public async Task GetSectionSeats_ResultNull_NotFound()
		{
			var mediator = MediatorMockObjectBuilder.Get<GetEventSectionSeatsQuery, IList<SeatDetails>>(null);
			var cache = DistributedCacheMockObjectProvider.Get();
			var controller = ControllerProvider.GetWithCache<EventsController>(mediator, cache);

			var result = await controller.GetSectionSeats(default, default);
			var value = result as NotFoundResult;

			Assert.NotNull(value);
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetSectionSeats_ResultNotNull_ReturnsAllData(GetValuesSuites suite)
		{
			var seats = ListGenerator.Generate<SeatDetails>(suite);
			var mediator = MediatorMockObjectBuilder.Get<GetEventSectionSeatsQuery, IList<SeatDetails>>(seats);
			var cache = DistributedCacheMockObjectProvider.Get();
			var controller = ControllerProvider.GetWithCache<EventsController>(mediator, cache);

			var result = await controller.GetSectionSeats(default, default);
			var value = (result as OkObjectResult)?.Value as Resource<IList<SeatDetails>>;

			Assert.NotNull(value?.Value);
			Assert.Equal(seats.Count, value.Value.Count);
		}
	}
}