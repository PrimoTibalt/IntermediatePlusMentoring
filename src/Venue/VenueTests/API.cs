using API.Abstraction.Helpers;
using DAL.Venues;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using TestsCore;
using VenueAPI.Controllers;
using VenueApplication.Queries;

namespace VenueTests
{
	public class API
	{
		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task Controller_GetAllVenues_ReturnsAllData(GetValuesSuites suite)
		{
			var venues = Suites.GetVenues(suite);
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetAllVenuesQuery>(), CancellationToken.None)).ReturnsAsync(venues);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetAllVenues(CancellationToken.None);
			var value = (result as OkObjectResult).Value as Resource<IList<Venue>>;
			
			Assert.NotNull(value);
			Assert.Equal(venues.Count, value.Value.Count);
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		public async Task Controller_GetVenueSectionsQuery_ReturnsCorrectSections(GetValuesSuites suite)
		{
			var sections = Suites.GetSections(suite);
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetVenueSectionsQuery>(), CancellationToken.None)).ReturnsAsync(sections);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetSectionsOfVenue(1);
			var value = (result as OkObjectResult).Value as Resource<IList<Section>>;

			Assert.NotNull(value);
			Assert.Equal(sections.Count, value.Value.Count);
		}

		[Fact]
		public async Task Controller_GetvenueSectionsQuery_ReturnsNotFound()
		{
			var sections = Suites.GetSections(GetValuesSuites.Null);
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetVenueSectionsQuery>(), CancellationToken.None)).ReturnsAsync(sections);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetSectionsOfVenue(1);

			Assert.IsType<NotFoundObjectResult>(result);
		}

		public static VenuesController GetController(IMediator mediator, LinkGenerator linkGenerator)
		{
			var controller = new VenuesController(mediator, linkGenerator);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}