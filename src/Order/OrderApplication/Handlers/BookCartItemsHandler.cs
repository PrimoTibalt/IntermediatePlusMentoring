using API.Abstraction.Helpers;
using DAL;
using DAL.Events;
using DAL.Infrastructure.Cache.Services;
using DAL.Orders;
using DAL.Orders.Repository;
using DAL.Payments;
using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Services;
using OrderApplication.Commands;
using System.Data;

namespace OrderApplication.Handlers
{
	public class BookCartItemsHandler(ICartRepository cartRepository,
		IGenericRepository<Payment, long> paymentRepository,
		ICacheService<EventSeat> seatsCacheService,
		IBookCartOperation bookCartOperation,
		INotificationService<(IList<CartItem> CartItems, long PaymentId)> notificationService,
		ILogger<BookCartItemsHandler> logger)
		: IRequestHandler<BookCartItemsCommand, Result<long?>>
	{
		private readonly ICartRepository _cartRepository = cartRepository;
		private readonly IGenericRepository<Payment, long> _paymentRepository = paymentRepository;
		private readonly ICacheService<EventSeat> _seatsCacheService = seatsCacheService;
		private readonly IBookCartOperation _bookCartOperation = bookCartOperation;
		private readonly INotificationService<(IList<CartItem> CartItems, long PaymentId)> _notificationService = notificationService;
		private readonly ILogger _logger = logger;

		public async Task<Result<long?>> Handle(BookCartItemsCommand request, CancellationToken cancellationToken)
		{
			// We don't actually need whole items here
			// It is a problem that should be fixed with tagging of cache entries
			// TODO: Add cache tags, delete request of all cart items
			var cartItems = await _cartRepository.GetItemsWithEventSeat(request.Id);
			if (cartItems == null)
				return null;

			await _cartRepository.BeginTransaction();

			var result = await _bookCartOperation.TryBookCart(request.Id, request.OptimisticExecution);
			if (!result) return FailedUnavailableSeats(request.Id);
			var payment = await CreatePayment(request.Id);

			await _paymentRepository.Save();
			await _cartRepository.CommitTransaction();

			try
			{
				await _seatsCacheService.Clean(cartItems.Select(ci => ci.EventSeat).ToList());
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Could not clean seats cache.");
			}

			await _notificationService.SendNotification(new(cartItems, payment.Id));
			return Result<long?>.Success(payment.Id);
		}

		private async Task<Payment> CreatePayment(Guid id)
		{
			var payment = new Payment
			{
				CartId = id,
				Status = (int)PaymentStatus.InProgress,
			};
			await _paymentRepository.Create(payment);
			return payment;
		}

		private static Result<long?> FailedUnavailableSeats(Guid id) 
			=> Result<long?>.Failure($"Cart with id '{id}' contains unavailable seats.");
	}
}