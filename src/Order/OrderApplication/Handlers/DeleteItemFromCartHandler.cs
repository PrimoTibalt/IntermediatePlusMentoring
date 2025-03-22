using API.Abstraction.Helpers;
using Entities.Events;
using MediatR;
using OrderApplication.Commands;
using OrderApplication.Repository;

namespace OrderApplication.Handlers
{
	public class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand, Result<EventSeat>>
	{
		private readonly ICartItemRepository _cartItemsRepository;

		public DeleteItemFromCartHandler(ICartItemRepository cartItemsRepository)
		{
			_cartItemsRepository = cartItemsRepository;
		}

		public async Task<Result<EventSeat>> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
		{
			var cartItem = await _cartItemsRepository.GetBy(request.CartId, request.EventId, request.SeatId);
			if (cartItem is null)
				return Result<EventSeat>.Failure($"Did not find item in cart '{request.CartId}' with event id '{request.EventId}' and seat id '{request.SeatId}'.");

			await _cartItemsRepository.Delete(cartItem.Id);

			var result = await _cartItemsRepository.Save();
			if (result == 0)
				return Result<EventSeat>.Failure("Delete did not succeed");

			if (cartItem.EventSeat.Status != (int)SeatStatus.Sold)
			{
				cartItem.EventSeat.Status = (int)SeatStatus.Available;
				await _cartItemsRepository.Save();
			}

			return Result<EventSeat>.Success(cartItem.EventSeat);
		}
	}
}