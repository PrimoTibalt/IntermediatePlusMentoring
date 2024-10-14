using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Orders.Repository;
using MediatR;

namespace OrderApplication.Carts
{
    public class DeleteItem
    {
        public class Command : IRequest<Result<Unit>>        
        {
            public Guid CartId { get; set; }
            public long SeatId { get; set; }
            public int EventId { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ICartItemRepository _cartItemsRepository;

            public RequestHandler(ICartItemRepository cartItemsRepository)
            {
                _cartItemsRepository = cartItemsRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
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

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}