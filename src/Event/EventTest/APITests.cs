using API.Abstraction.Helpers;
using DAL.Events;
using EventAPI.Controllers;
using EventApplication.Entities;
using EventApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using TestsCore;

namespace EventTest
{
	public class APITests
	{
		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetAll_ResultNotNull_ReturnsAllData(GetValuesSuites suite)
		{
			var events = Suites.GetEvents(suite);
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetAllEventsQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(events);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetAll(CancellationToken.None);
			var value = (result as OkObjectResult).Value as Resource<IList<Event>>;

			Assert.NotNull(value?.Value);
			Assert.Equal(events.Count, value.Value.Count);
		}

		[Fact]
		public async Task GetSectionSeats_ResultNull_NotFound()
		{
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetEventSectionSeatsQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((IList<SeatDetails>)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetSectionSeats(1, 1);
			var value = result as NotFoundResult;

			Assert.NotNull(value);
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetSectionSeats_ResultNotNull_ReturnsAllData(GetValuesSuites suite)
		{
			var seats = Suites.GetSeats(suite);
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetEventSectionSeatsQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(seats);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetSectionSeats(1, 1);
			var value = (result as OkObjectResult)?.Value as Resource<IList<SeatDetails>>;

			Assert.NotNull(value?.Value);
			Assert.Equal(seats.Count, value.Value.Count);
		}

		private static EventsController GetController(IMediator mediator, LinkGenerator linkGenerator)
		{
			var controller = new EventsController(mediator, linkGenerator);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}