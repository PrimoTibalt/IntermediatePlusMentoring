using API.Abstraction.Helpers;
using DAL.Events;
using EventApplication.Entities;
using EventApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
		[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
		public async Task<IActionResult> GetAll(CancellationToken token)
		{
			var result = await _mediator.Send(new GetAllEventsQuery(), token);
			var resource = new Resource<IList<Event>>
			{
				Value = result,
				Links = []
			};
			return Ok(resource);
		}

		[HttpGet("{eventId}/sections/{sectionId}/seats")]
		[ProducesResponseType(typeof(Resource<IList<SeatDetails>>), 200)]
		[ProducesResponseType(404)]
		[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
		public async Task<IActionResult> GetSectionSeats(int eventId, int sectionId)
		{
			var result = await _mediator.Send(new GetEventSectionSeatsQuery { EventId = eventId, SectionId = sectionId });
			if (result is null) return NotFound();
		
			var resource = new Resource<IList<SeatDetails>>
			{
				Value = result,
				Links = [
					new Link {
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetAll), values: new {id = eventId}),
						Method = "GET"
					}
				]
			};
			return Ok(resource);
		}
	}
}
