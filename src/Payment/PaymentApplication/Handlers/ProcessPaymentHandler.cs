using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Publishers;
using PaymentApplication.Commands;
using PaymentApplication.Notifications;

namespace PaymentApplication.Handlers
{
	public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, bool>
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly IPersistentNotificationPublisher _notificationsPublisher;

		public ProcessPaymentHandler(IPaymentRepository paymentRepository, IPersistentNotificationPublisher notificationsPublisher)
		{
			_paymentRepository = paymentRepository;
			_notificationsPublisher = notificationsPublisher;
		}

		public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = await _paymentRepository.GetById(request.Id);
			if (payment is null || payment.Status != (int)PaymentStatus.InProgress)
				return false;

			string queueName;
			Func<long, Task<bool>> task;
			if (request.Complete)
			{
				queueName = KnownQueueNames.PaymentCompleted;
				task = _paymentRepository.CompletePayment;
			}
			else
			{
				queueName = KnownQueueNames.PaymentFailed;
				task = _paymentRepository.FailPayment;
			}

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
				try
				{
					var details = await _paymentRepository.GetPaymentWithRelatedInfo(request.Id);
					var notification = PaymentProcessedNotificationProducer.Get(details, request.Complete);
					await _notificationsPublisher.PersistentPublish(notification, queueName);
				}
				catch 
				{
					// logging
				}
			}

			return result;
		}
	}
}