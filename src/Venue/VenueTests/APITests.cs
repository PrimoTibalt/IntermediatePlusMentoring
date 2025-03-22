using API.Abstraction.Helpers;
using DAL.Venues;
using Entities.Venues;
using Microsoft.AspNetCore.Mvc;
using TestsCore;
using TestsCore.Providers;
using VenueAPI.Controllers;
using VenueApplication.Entities;
using VenueApplication.Queries;

namespace VenueTests
{
	public class APITests
	{
		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetAllVenues_ResultNotNull_ReturnsAllData(GetValuesSuites suite)
		{
			var venues = ListGenerator.Generate<Venue>(suite);
			var mediator = MediatorMockObjectBuilder.Get<GetAllVenuesQuery, IList<Venue>>(venues);
			var controller = ControllerProvider.Get<VenuesController>(mediator);

			var result = await controller.GetAllVenues(CancellationToken.None);
			var value = (result as OkObjectResult).Value as Resource<IList<Venue>>;
			
			Assert.NotNull(value);
			Assert.Equal(venues.Count, value.Value.Count);
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task GetVenueSectionsQuery_ResultNotNull_ReturnsCorrectSections(GetValuesSuites suite)
		{
			var sections = ListGenerator.Generate<SectionDetails>(suite);
			var mediator = MediatorMockObjectBuilder.Get<GetVenueSectionsQuery, IList<SectionDetails>>(sections);
			var controller = ControllerProvider.Get<VenuesController>(mediator);

			var result = await controller.GetSectionsOfVenue(1);
			var value = (result as OkObjectResult).Value as Resource<IList<SectionDetails>>;

			Assert.NotNull(value);
			Assert.Equal(sections.Count, value.Value.Count);
		}

		[Fact]
		public async Task GetVenueSectionsQuery_ResultNull_ReturnsNotFound()
		{
			var sections = ListGenerator.Generate<SectionDetails>(GetValuesSuites.Null);
			var mediator = MediatorMockObjectBuilder.Get<GetVenueSectionsQuery, IList<SectionDetails>>(sections);
			var controller = ControllerProvider.Get<VenuesController>(mediator);

			var result = await controller.GetSectionsOfVenue(1);

			Assert.IsType<NotFoundObjectResult>(result);
		}
	}
}