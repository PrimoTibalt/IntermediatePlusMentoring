using DAL.Venues;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VenueAPI.Helpers;
using VenueApplication.Entities;
using Venues = VenueApplication.Venues;
using Sections = VenueApplication.Sections;

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
		[ProducesResponseType(typeof(IList<Venue>), 200)]
		public async Task<IActionResult> GetAllVenues(CancellationToken token)
		{
			var result = await _mediator.Send(new Venues.List.Query(), token);
			var resource = new Resource<IList<Venue>>
			{
				Value = result,
				Links = GenerateLinks(result)
			};

			return Ok(resource);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(VenueDetails), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _mediator.Send(new Venues.Details.Query { Id = id });
			if (result is not null)
			{
				var resource = new Resource<VenueDetails>
				{
					Value = result,
					Links = GenerateLinks(result)
				};
				return Ok(resource);
			}

			return NotFound();
		}

		[HttpGet("{venueId}/sections")]
		public async Task<IActionResult> GetSectionsOfVenue(int venueId)
		{
			var result = await _mediator.Send(new Sections.List.Query { VenueId = venueId });
			var resource = new Resource<IList<Section>>
			{
				Value = result,
				Links = GenerateLinks(result)
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
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { item.Id }),
					Method = "GET"
				});
			}
			return links;
		}

		private IList<Link> GenerateLinks(VenueDetails details)
		{
			var links = new List<Link>
			{
				new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetSectionsOfVenue), values: new { venueId = details.Id }),
					Method = "GET"
				}
			};
			return links;
		}


		private IList<Link> GenerateLinks(IList<Section> sections)
		{
			var links = new List<Link>();
			var venues = sections.Select(s => s.VenueId).ToHashSet();
			foreach (var venueId in venues)
			{
				links.Add(new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { id = venueId }),
					Method = "GET"
				});
			}
			return links;
		}
	}
}
