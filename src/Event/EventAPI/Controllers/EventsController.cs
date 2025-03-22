using API.Abstraction.Cache;
using API.Abstraction.Helpers;
using Cache.Infrastructure;
using EventAPI.DistributedCacheModels;
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
		private readonly bool _isCachingEnabled;

		public EventsController(IMediator mediator, LinkGenerator linkGenerator, IDistributedCache cache)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
			_isCachingEnabled = !string.Equals(Environment.GetEnvironmentVariable("CACHING_DISABLED"), "true", StringComparison.OrdinalIgnoreCase);
			_cache = cache;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Resource<IList<ProtoEvent>>), 200)]
		[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
		public async Task<IActionResult> GetAll(CancellationToken token)
		{
			var getResult = async () => (await _mediator.Send(new GetAllEventsQuery(), token)).Select(ProtoEvent.Create).ToList();
			var result = await (_isCachingEnabled ? _cache.GetOrCreate(EventCacheKeysTemplates.AllEventsCacheKey, getResult, token) : getResult());

			var resource = new Resource<IList<ProtoEvent>>
			{
				Value = result,
				Links = []
			};
			return Ok(resource);
		}

		[HttpGet("{eventId}/sections/{sectionId}/seats")]
		[ProducesResponseType(typeof(Resource<IList<ProtoSeatDetails>>), 200)]
		[ProducesResponseType(404)]
		[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
		public async Task<IActionResult> GetSectionSeats(int eventId, int sectionId)
		{
			var cacheKey = string.Format(EventCacheKeysTemplates.EventAppEventSeatsByEventIdSectionIdCacheTemplate, eventId, sectionId);
			var getResult = async () => (await _mediator.Send(new GetEventSectionSeatsQuery { EventId = eventId, SectionId = sectionId }))
				.Select(ProtoSeatDetails.Create).ToList();
			var result = await (_isCachingEnabled ? _cache.GetOrCreate(cacheKey, getResult, new(), CancellationToken.None) : getResult());

			if (result is null) return NotFound();
		
			var resource = new Resource<IList<ProtoSeatDetails>>
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
