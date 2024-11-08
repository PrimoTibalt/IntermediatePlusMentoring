using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Infrastructure.Cache.Services;
using DAL.Orders.Repository;
using MediatR;
using OrderApplication.Commands;

namespace OrderApplication.Handlers
{
	public class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand, Result<Unit>>
	{
		private readonly ICartItemRepository _cartItemsRepository;
		private readonly ICacheService<EventSeat> _seatsCacheService;

		public DeleteItemFromCartHandler(ICartItemRepository cartItemsRepository, ICacheService<EventSeat> seatsCacheService)
		{
			_cartItemsRepository = cartItemsRepository;
			_seatsCacheService = seatsCacheService;
		}

		public async Task<Result<Unit>> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
		{
			var cartItem = await _cartItemsRepository.GetBy(request.CartId, request.EventId, request.SeatId);
			if (cartItem is null)
				return Result<Unit>.Failure($"Did not find item in cart '{request.CartId}' with event id '{request.EventId}' and seat id '{request.SeatId}'.");

			await _cartItemsRepository.Delete(cartItem.Id);

			var result = await _cartItemsRepository.Save();
			if (result == 0)
				return Result<Unit>.Failure("Delete did not succeed");

			if (cartItem.EventSeat.Status != (int)SeatStatus.Sold)
			{
				cartItem.EventSeat.Status = (int)SeatStatus.Available;
				await _cartItemsRepository.Save();
			}

			await _seatsCacheService.Clean([cartItem.EventSeat]);
			return Result<Unit>.Success(Unit.Value);
		}
	}
}