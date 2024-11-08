using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Orders;
using DAL.Orders.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApplication.Commands;
using IEventSeatRepository = DAL.Orders.Repository.IEventSeatRepository;

namespace OrderApplication.Handlers
{
    public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, Result<Unit>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IEventSeatRepository _seatRepository;

        public AddItemToCartHandler(ICartRepository cartRepository, IEventSeatRepository seatRepository, ICartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _seatRepository = seatRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Result<Unit>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
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

            var seat = await _seatRepository.GetBy(request.EventId, request.SeatId);
            if (seat is null)
                return Result<Unit>.Failure($"Seat with id '{request.SeatId}' for event id '{request.EventId}' doesn't exist.");
            if (seat.Status != (int)SeatStatus.Available)
                return Result<Unit>.Failure($"Can't add seat with id '{request.SeatId}' to cart because it is already booked or sold.");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                EventSeatId = request.SeatId,
                PriceId = seat.PriceId
            };

            int result;
            try
            {
                await _cartItemRepository.Create(cartItem);
                result = await _cartItemRepository.Save();
            }
            catch (DbUpdateException)
            {
                return Result<Unit>.Failure("Seat is already in the cart");
            }

            if (result == 1) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Cart item was not created.");
        }
    }
}