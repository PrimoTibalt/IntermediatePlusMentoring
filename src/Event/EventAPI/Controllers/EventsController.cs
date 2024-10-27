using API.Abstraction.Cache;
using API.Abstraction.Helpers;
using DAL.Events;
using EventApplication.Entities;
using EventApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace EventAPI.Controllers
{
	[Route("events")]
	[ApiController]
	[Produces("application/json")]
	public class EventsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;
		private readonly IDistributedCache _cache;

		public EventsController(IMediator mediator, LinkGenerator linkGenerator, IDistributedCache cache)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
			_cache = cache;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Resource<IList<Event>>), 200)]
		[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
		public async Task<IActionResult> GetAll(CancellationToken token)
		{
			var result = await _cache.GetOrCreate(EventCacheKeysTemplates.AllEventsCacheKey, async () =>
				{
					return await _mediator.Send(new GetAllEventsQuery(), token);
				},
				token);

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
			var cacheKey = string.Format(EventCacheKeysTemplates.EventAppEventSeatsByEventIdSectionIdCacheTemplate, eventId, sectionId);
			var result = await _cache.GetOrCreate(cacheKey, async () =>
				{
					return await _mediator.Send(new GetEventSectionSeatsQuery { EventId = eventId, SectionId = sectionId });
				},
				new(),
				CancellationToken.None);

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
