using API.Abstraction.Helpers;
using DAL;
using DAL.Events;
using DAL.Infrastructure.Cache.Services;
using DAL.Orders.Repository;
using DAL.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApplication.Commands;

namespace OrderApplication.Handlers
{
	public class BookCartItemsHandler : IRequestHandler<BookCartItemsCommand, Result<long?>>
	{
		private readonly IEventSeatRepository _seatRepository;
		private readonly ICartRepository _cartRepository;
		private readonly IGenericRepository<Payment, long> _paymentRepository;
		private readonly ICacheService<EventSeat> _seatsCacheService;

		public BookCartItemsHandler(IEventSeatRepository seatRepository,
			ICartRepository cartRepository,
			IGenericRepository<Payment, long> paymentRepository,
			ICacheService<EventSeat> seatsCacheService)
		{
			_seatRepository = seatRepository;
			_cartRepository = cartRepository;
			_paymentRepository = paymentRepository;
			_seatsCacheService= seatsCacheService;
		}

		public async Task<Result<long?>> Handle(BookCartItemsCommand request, CancellationToken cancellationToken)
		{
			var seats = await _cartRepository.GetItemsWithEventSeat(request.Id);
			if (seats is null) return null;
			if (seats.Count == 0) return Result<long?>.Failure($"Cart with id '{request.Id}' is empty.");

			foreach (var seat in seats)
			{
				seat.EventSeat.Status = SeatStatus.Booked.ToString().ToLowerInvariant();
				_seatRepository.Update(seat.EventSeat);
			}

			var result = await _seatRepository.Save();
			if (result > 0)
			{
				var payment = new Payment
				{
					CartId = request.Id,
					Status = PaymentStatus.InProgress.ToString().ToLowerInvariant(),
				};
				try
				{
					await _paymentRepository.Create(payment);
					await _paymentRepository.Save();
				}
				catch (DbUpdateException)
				{
					return Result<long?>.Failure("Payment already exists");
				}

				await _seatsCacheService.Clean(seats.Select(s => s.EventSeat).ToList());
				return Result<long?>.Success(payment.Id);
			}
			else
			{
				return Result<long?>.Failure($"Seats from cart with id '{request.Id}' are already booked.");
			}
		}
	}
}