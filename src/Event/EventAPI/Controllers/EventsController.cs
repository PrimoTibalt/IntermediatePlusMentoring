using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Venues;
using EventApplication.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Seats = EventApplication.Seats;
using Sections = EventApplication.Sections;
using Events = EventApplication.Events;

namespace EventAPI.Controllers
{
	[Route("events")]
	[ApiController]
	[Produces("application/json")]
	public class EventsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;

		public EventsController(IMediator mediator, LinkGenerator linkGenerator)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Resource<IList<Event>>), 200)]
		public async Task<IActionResult> GetAll(CancellationToken token)
		{
			var result = await _mediator.Send(new Events.List.Query(), token);
			var resource = new Resource<IList<Event>>
			{
				Value = result,
				Links = GenerateLinks(result)
			};
			return Ok(resource);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Resource<EventDetails>), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _mediator.Send(new Events.Details.Query { Id = id });
			if (result is null)
				return NotFound();

			var resource = new Resource<EventDetails>
			{
				Value = result,
				Links = []
			};
			return Ok(resource);
		}

		[HttpGet("{id}/sections")]
		[ProducesResponseType(typeof(Resource<IList<Section>>), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetEventSections(int id)
		{
			var result = await _mediator.Send(new Sections.List.Query { Id = id });
			if (result is null) return NotFound();

			var resource = new Resource<IList<Section>>
			{
				Value = result,
				Links = [new Link { Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { id }), Method = "GET" }]
			};
			return Ok(resource);
		}

		[HttpGet("{eventId}/sections/{sectionId}/seats")]
		[ProducesResponseType(typeof(Resource<IList<SeatDetails>>), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetSectionSeats(int eventId, int sectionId)
		{
			var result = await _mediator.Send(new Seats.List.Query { EventId = eventId, SectionId = sectionId });
			if (result is null) return NotFound();
		
			var resource = new Resource<IList<SeatDetails>>
			{
				Value = result,
				Links = [
					new Link {
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new {id = eventId}),
						Method = "GET"
					},
					new Link {
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetEventSections), values: new { id = eventId }),
						Method = "GET"
					}		
				]
			};
			return Ok(resource);
		}

		private IList<Link> GenerateLinks(IList<Event> result)
		{
			var links = new List<Link>();
			foreach (var item in result)
			{
				links.Add(new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { id = item.Id }),
					Method = "GET"
				});
			}
			return links;
		}

		private IList<Link> GenerateLinks(EventDetails result)
		{
			var links = new List<Link>
			{
				new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetAll)),
					Method = "GET"
				},
				new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetEventSections), values: new { id = result.Id }),
					Method = "GET"
				}
			};

			return links;
		}
	}
}
