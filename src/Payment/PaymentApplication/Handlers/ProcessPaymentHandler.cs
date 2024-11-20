using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Services;
using PaymentApplication.Commands;

namespace PaymentApplication.Handlers
{
	public class ProcessPaymentHandler(IPaymentRepository paymentRepository,
		INotificationService<long> notificationService,
		ILogger<ProcessPaymentHandler> logger)
		: IRequestHandler<ProcessPaymentCommand, bool>
	{
		private readonly IPaymentRepository _paymentRepository = paymentRepository;
		private readonly INotificationService<long> _notificationService = notificationService;
		private readonly ILogger _logger = logger;

		public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = await _paymentRepository.GetById(request.Id);
			if (payment is null || payment.Status != (int)PaymentStatus.InProgress)
				return false;

			Func<long, Task<bool>> task = request.Complete ? 
				_paymentRepository.CompletePayment :
				_paymentRepository.FailPayment;

			bool result;
			try
			{
				result = await task(request.Id);
			}
			catch
			{
				result = false;
			}

			if (result)
			{
				await _notificationService.SendNotification(request.Id);
			}

			return result;
		}
	}
}