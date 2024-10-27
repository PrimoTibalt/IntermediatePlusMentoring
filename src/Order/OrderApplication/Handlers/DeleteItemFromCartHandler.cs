using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Orders.Repository;
using MediatR;
using OrderApplication.Cache;
using OrderApplication.Commands;

namespace OrderApplication.Handlers
{
	public class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand, Result<Unit>>
	{
		private readonly ICartItemRepository _cartItemsRepository;
		private readonly ICacheCleaner _cacheCleaner;

		public DeleteItemFromCartHandler(ICartItemRepository cartItemsRepository, ICacheCleaner cacheCleaner)
		{
			_cartItemsRepository = cartItemsRepository;
			_cacheCleaner = cacheCleaner;
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

			if (cartItem.EventSeat.Status != SeatStatus.Sold.ToString().ToLowerInvariant())
			{
				cartItem.EventSeat.Status = SeatStatus.Available.ToString().ToLowerInvariant();
				await _cartItemsRepository.Save();
			}

			await _cacheCleaner.CleanEventSeatsCache([cartItem.EventSeat]);
			return Result<Unit>.Success(Unit.Value);
		}
	}
}