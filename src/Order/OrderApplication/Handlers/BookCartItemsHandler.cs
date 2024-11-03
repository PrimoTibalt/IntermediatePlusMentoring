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
			return request.OptimisticExecution ? 
				await OptimisticExecution(request.Id, cancellationToken) :
				await PessimisticExecution(request.Id, cancellationToken);
		}

		private async Task<Result<long?>> OptimisticExecution(Guid id, CancellationToken token)
		{
			await _cartRepository.BeginTransaction();
			var cartItems = await _cartRepository.GetItemsWithEventSeat(id);
			if (cartItems is null || cartItems.Count == 0) return null;
			if (cartItems.Any(ci => !string.Equals(ci.EventSeat.Status, SeatStatus.Available.ToString(),
				StringComparison.OrdinalIgnoreCase)))
				return FailedUnavailableSeats(id);
			var seats = _seatRepository.GetByIdsQueryable(cartItems.Select(ci => ci.EventSeatId).ToArray());
			var failed = false;
			foreach (var cartItem in cartItems)
			{
				var seat = seats.Where(es => es.Id == cartItem.EventSeatId && es.Version == cartItem.EventSeat.Version);
				var result = await seat.ExecuteUpdateAsync(p =>
					p.SetProperty(es => es.Status, SeatStatusStrings.Booked)
					.SetProperty(es => es.Version, cartItem.EventSeat.Version + 1), token);
				if (result != 1)
				{
					failed = true;
					break;
				}
			}

			if (failed)
			{
				await _cartRepository.RollbackTransaction();
				return FailedUnavailableSeats(id);
			}

			var payment = await CreatePayment(id);

			await _seatRepository.Save();
			await _cartRepository.CommitTransaction();
			return Result<long?>.Success(payment.Id);
		}

		private async Task<Result<long?>> PessimisticExecution(Guid id, CancellationToken token)
		{
			Result<long?> result = null;
			try
			{
				await _cartRepository.BeginTransaction();
				var cartItems = await _cartRepository.GetItemsWithEventSeat(id);
				if (cartItems is null || cartItems.Count == 0) return null;
				if (cartItems.Any(ci => !string.Equals(ci.EventSeat.Status, SeatStatus.Available.ToString(),
					StringComparison.OrdinalIgnoreCase)))
					return FailedUnavailableSeats(id);
				var seats = await _seatRepository.GetByIds(cartItems.Select(ci => ci.EventSeatId).ToArray());

				foreach (var seat in seats)
				{
					seat.Status = SeatStatusStrings.Booked;
				}

				var payment = await CreatePayment(id);
				var updateCount = await _seatRepository.Save();

				if (updateCount < cartItems.Count)
				{
					await _cartRepository.RollbackTransaction();
					return FailedUnavailableSeats(id);
				}

				await _cartRepository.CommitTransaction();
				result = Result<long?>.Success(payment.Id);
				await _seatsCacheService.Clean(cartItems.Select(s => s.EventSeat).ToList());
			}
			catch (InvalidOperationException)
			{
				await _cartRepository.RollbackTransaction();
			}

			return result;
		}

		private async Task<Payment> CreatePayment(Guid id)
		{
			var payment = new Payment
			{
				CartId = id,
				Status = PaymentStatus.InProgress.ToString().ToLowerInvariant(),
			};
			await _paymentRepository.Create(payment);
			return payment;
		}

		private static Result<long?> FailedUnavailableSeats(Guid id) 
			=> Result<long?>.Failure($"Cart with id '{id}' contains unavailable seats.");
	}
}