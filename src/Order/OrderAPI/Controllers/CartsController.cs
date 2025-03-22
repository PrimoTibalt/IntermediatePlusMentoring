using API.Abstraction.Helpers;
using Cache.Infrastructure.Services;
using Entities.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications.Infrastructure.Services;
using OrderAPI.DTOs;
using OrderApplication.Commands;
using OrderApplication.Entities;
using OrderApplication.Queries;
using OrderApplication.Repository;

namespace OrderAPI.Controllers
{
	[ApiController]
	[Route("orders/carts")]
	public class CartsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;
		private readonly ICacheService<EventSeat> _cacheService;
		private readonly IServiceProvider _serviceProvider;
		private readonly INotificationService<long> _notificationService;
		private readonly ILogger _logger;

		public CartsController(IMediator mediator, LinkGenerator linkGenerator, ICacheService<EventSeat> cacheService, IServiceProvider serviceProvider, ILogger<CartsController> logger, INotificationService<long> notificationService)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
			_cacheService = cacheService;
			_serviceProvider = serviceProvider;
			_logger = logger;
			_notificationService = notificationService;
		}

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(Resource<CartDetails>), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetDetails(Guid id)
		{
			var details = await _mediator.Send(new GetCartQuery { Id = id });
			if (details is null) return NotFound();
			var resource = GetResource(details, id);
			return Ok(resource);
		}

		[HttpPut("{id:guid}/book-pessimistic")]
		[ProducesResponseType(typeof(Resource<string>), 200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> BookCartItemsPessimistic(Guid id)
		{
			return await BookCartItems(id, false);
		}

		[HttpPut("{id:guid}/book-optimistic")]
		[ProducesResponseType(typeof(Resource<string>), 200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> BookCartItemsOptimistic(Guid id)
		{
			return await BookCartItems(id, true);
		}

		private async Task<IActionResult> BookCartItems(Guid id, bool optimisticExecution)
		{
			var result = await _mediator.Send(new BookCartItemsCommand { Id = id, OptimisticExecution = optimisticExecution });
			if (result is null) return NotFound($"Cart with id '{id}' doesn't exist.");
			if (result.Error) return BadRequest(result.ErrorMessage);
			_ = Task.Run(async () =>
			{
				try
				{
					await _notificationService.SendNotification(result.Value.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unable to send notification");
				}

				using var scope = _serviceProvider.CreateAsyncScope();
				try
				{
					var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();
					var cartItems = await cartRepository.GetItemsWithEventSeat(id);
					await _cacheService.CleanAsync([.. cartItems.Select(ci => ci.EventSeat)]);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unable to clean event seats' cache");
				}
			});

			var resource = new Resource<string>
			{
				Value = $"Payment with id '{result.Value}' was created",
				Links = []
			};
			return Ok(resource);
		}

		[HttpPost("{id:guid}")]
		[ProducesResponseType(typeof(Resource<CartDetails>), 200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> AddToCart(CartItemInputModel item, [FromRoute] Guid id)
		{
			var result = await _mediator.Send(new AddItemToCartCommand { CartId = id, EventId = item.EventId, SeatId = item.SeatId, UserId = item.UserId });
			if (result is null) return NotFound("Seat not found");
			if (result.Error) return BadRequest(result.ErrorMessage);
			var details = await _mediator.Send(new GetCartQuery { Id = id });
			var resource = GetResource(details, id);
			return Ok(resource);
		}

		[HttpDelete("{cartId:guid}/events/{eventId}/seats/{seatId:long}")]
		[ProducesResponseType(typeof(Resource<string>), 200)]
		public async Task<IActionResult> DeleteFromCart(Guid cartId, int eventId, long seatId)
		{
			var result = await _mediator.Send(new DeleteItemFromCartCommand { CartId = cartId, EventId = eventId, SeatId = seatId });
			var resource = new Resource<string>();
			if (result.Error)
				resource.Value = result.ErrorMessage;
			else
				resource.Value = "Deleted.";

			await _cacheService.CleanAsync([result.Value]);
			resource.Links = [
				new Link
				{
					Href = _linkGenerator.GetUriByAction(HttpContext, nameof(GetDetails), values: new { id = cartId }),
					Method = "GET"
				},
				GetLinkToBooking(cartId)
			];

			return Ok(resource);
		}

		private Resource<CartDetails> GetResource(CartDetails details, Guid id)
		{
			var resource = new Resource<CartDetails>
			{
				Value = details,
				Links = [
					new Link
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(AddToCart), values: new { id }),
						Method = "POST"
					},
					GetLinkToBooking(id)
				]
			};
			foreach (var cartItem in details.Items)
			{
				resource.Links.Add(
					new Link
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, nameof(DeleteFromCart), values: new { cartId = id, eventId = cartItem.EventSeat.EventId, seatId = cartItem.EventSeat.Id }),
						Method = "DELETE"
					}
				);
			}

			return resource;
		}

		private Link GetLinkToBooking(Guid id)
		{
			var link = new Link
			{
				Href = _linkGenerator.GetUriByAction(HttpContext, nameof(BookCartItems), values: new { id }),
				Method = "PUT"
			};
			return link;
		}
	}
}