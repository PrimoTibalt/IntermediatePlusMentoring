using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Services;
using PaymentApplication.Commands;

namespace PaymentApplication.Handlers
{
	public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>
	{
		private readonly IDapperPaymentRepository _paymentRepository;
		private readonly INotificationService<long> _notificationService;
		private readonly ILogger _logger;

		public ProcessPaymentHandler(
			IDapperPaymentRepository paymentRepository,
			INotificationService<long> notificationService,
			ILogger<ProcessPaymentHandler> logger)
		{
			_paymentRepository = paymentRepository;
			_notificationService = notificationService;
			_logger = logger;
		}

		public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = await _paymentRepository.GetById(request.Id);
			if (payment is null || payment.Status != (int)PaymentStatus.InProgress)
				return null;

			Func<long, Task<bool>> task = request.Complete ?
				_paymentRepository.CompletePayment :
				_paymentRepository.FailPayment;

			bool result;
			try
			{
				result = await task(request.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Operation on payment with id '{Id}' failed.", request.Id);
				result = false;
			}

			if (result)
			{
				await _notificationService.SendNotification(request.Id);
			}

			return new() { Success = result };
		}
	}
}