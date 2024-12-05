using API.Abstraction.Helpers;
using DAL.Events;
using DAL.Infrastructure.Cache.Services;
using DAL.Orders.Repository;
using DAL.Payments;
using MediatR;
using Notifications.Infrastructure.Services;
using OrderApplication.Commands;

namespace OrderApplication.Handlers
{
	public class BookCartItemsHandler(ICartRepository cartRepository,
		IPaymentRepository paymentRepository,
		ICacheService<EventSeat> seatsCacheService,
		IBookCartOperation bookCartOperation,
		INotificationService<long> notificationService)
		: IRequestHandler<BookCartItemsCommand, Result<long?>>
	{
		private readonly ICartRepository _cartRepository = cartRepository;
		private readonly IPaymentRepository _paymentRepository = paymentRepository;
		private readonly ICacheService<EventSeat> _seatsCacheService = seatsCacheService;
		private readonly IBookCartOperation _bookCartOperation = bookCartOperation;
		private readonly INotificationService<long> _notificationService = notificationService;

		public async Task<Result<long?>> Handle(BookCartItemsCommand request, CancellationToken cancellationToken)
		{
			await _cartRepository.BeginTransaction();

			var result = await _bookCartOperation.TryBookCart(request.Id, request.OptimisticExecution);
			if (!result) return FailedUnavailableSeats(request.Id);
			var payment = await CreatePayment(request.Id);

			await _paymentRepository.Save();
			await _cartRepository.CommitTransaction();

			await _seatsCacheService.Clean(request.Id);
			await _notificationService.SendNotification(payment.Id);
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