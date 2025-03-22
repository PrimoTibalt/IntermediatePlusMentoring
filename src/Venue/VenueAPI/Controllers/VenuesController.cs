using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Abstraction.Helpers;
using VenueApplication.Queries;
using VenueApplication.Entities;
using Entities.Venues;

namespace VenueAPI.Controllers
{
	[Route("venues")]
	[ApiController]
	[Produces("application/json")]
	public class VenuesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;

		public VenuesController(IMediator mediator, LinkGenerator linkGenerator)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Resource<IList<Venue>>), 200)]
		public async Task<IActionResult> GetAllVenues(CancellationToken token)
		{
			var result = await _mediator.Send(new GetAllVenuesQuery(), token);
			var resource = new Resource<IList<Venue>>
			{
				Value = result,
				Links = GenerateLinks(result)
			};

			return Ok(resource);
		}

		[HttpGet("{venueId}/sections")]
		[ProducesResponseType(typeof(Resource<IList<SectionDetails>>), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetSectionsOfVenue(int venueId)
		{
			var result = await _mediator.Send(new GetVenueSectionsQuery { VenueId = venueId });
			if (result is null)
				return NotFound($"Venue with id '{venueId}' doesn't exist.");

			var resource = new Resource<IList<SectionDetails>>
			{
				Value = result,
				Links = 
				[
					new Link 
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetAllVenues)),
						Method = "GET"
					}
				]
			};

			return Ok(resource);
		}

		private IList<Link> GenerateLinks(IList<Venue> venues)
		{
			var links = new List<Link>();
			foreach (var item in venues)
			{
				links.Add(new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetSectionsOfVenue), values: new { venueId = item.Id }),
					Method = "GET"
				});
			}
			return links;
		}
	}
}
