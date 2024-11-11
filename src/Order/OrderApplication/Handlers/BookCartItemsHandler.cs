using API.Abstraction.Helpers;
using DAL;
using DAL.Events;
using DAL.Infrastructure.Cache.Services;
using DAL.Orders.Repository;
using DAL.Payments;
using MediatR;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Publishers;
using OrderApplication.Commands;
using OrderApplication.Services;
using System.Data;

namespace OrderApplication.Handlers
{
	public class BookCartItemsHandler : IRequestHandler<BookCartItemsCommand, Result<long?>>
	{
		private readonly ICartRepository _cartRepository;
		private readonly IGenericRepository<Payment, long> _paymentRepository;
		private readonly ICacheService<EventSeat> _seatsCacheService;
		private readonly IBookCartOperation _bookCartOperation;
		private readonly INotificationsPublisher _notificationsPublisher;

		public BookCartItemsHandler(IEventSeatRepository seatRepository,
			ICartRepository cartRepository,
			IGenericRepository<Payment, long> paymentRepository,
			ICacheService<EventSeat> seatsCacheService,
			IBookCartOperation bookCartOperation,
			INotificationsPublisher notificationsPublisher)
		{
			_cartRepository = cartRepository;
			_paymentRepository = paymentRepository;
			_seatsCacheService = seatsCacheService;
			_bookCartOperation = bookCartOperation;
			_notificationsPublisher = notificationsPublisher;
		}

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

			await _notificationsPublisher.SendProtoSerializedMessage(BookingNotificationProducer.Get(cartItems, payment.Id),
				KnownQueueNames.Booking);
			await _seatsCacheService.Clean(cartItems.Select(ci => ci.EventSeat).ToList());
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