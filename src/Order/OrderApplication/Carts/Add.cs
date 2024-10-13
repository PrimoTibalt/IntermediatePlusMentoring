using API.Abstraction.Helpers;
using DAL;
using DAL.Events;
using DAL.Orders;
using DAL.Orders.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrderApplication.Carts
{
    public class Add
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid CartId { get; set; }
            public int EventId { get; set; }
            public long SeatId { get; set; }
            public int UserId { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ICartRepository _cartRepository;
            private readonly IGenericRepository<CartItem, long> _cartItemRepository;
            private readonly IGenericRepository<EventSeat, long> _seatRepository;

            public RequestHandler(ICartRepository repository, IGenericRepository<EventSeat, long> seatRepository, IGenericRepository<CartItem, long> cartItemRepository)
            {
                _cartRepository = repository;
                _seatRepository = seatRepository;
                _cartItemRepository = cartItemRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var cart = await _cartRepository.GetById(request.CartId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        Id = request.CartId,
                        UserId = request.UserId
                    };
                    await _cartRepository.Create(cart);
                    try
                    {
                        await _cartRepository.Save();
                    }
                    catch (DbUpdateException)
                    {
                        return Result<Unit>.Failure($"User with id '{request.UserId}' doesn't exist.");
                    }
                }
                var seat = await _seatRepository.GetById(request.SeatId);
                if (seat is null)
                    return Result<Unit>.Failure($"Seat with id '{request.SeatId}' doesn't exist.");
                if (Enum.Parse<SeatStatus>(seat.Status, true) != SeatStatus.Available)
                    return Result<Unit>.Failure($"Can't add seat with id '{request.SeatId}' to cart because it is already booked or sold.");

                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    EventSeatId = request.SeatId,
                    PriceId = seat.PriceId
                };
                await _cartItemRepository.Create(cartItem);
                var result = await _cartItemRepository.Save();
                if (result == 1) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Cart was not created.");
            }
        }
    }
}