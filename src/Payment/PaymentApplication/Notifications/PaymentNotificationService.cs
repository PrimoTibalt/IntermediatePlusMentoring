using API.Abstraction.Notifications;
using DAL.Payments.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Publishers;
using Notifications.Infrastructure.Services;

namespace PaymentApplication.Notifications
{
	public class PaymentNotificationService(IPersistentNotificationPublisher publisher,
		IServiceScopeFactory serviceScopeFactory,
		ILogger<PaymentNotificationService> logger)
		: NotificationService<long>(publisher, logger)
	{
		protected override async Task<Notification> GetNotification(long input)
		{
			using var scope = serviceScopeFactory.CreateScope();
			var paymentRepository = scope.ServiceProvider.GetRequiredService<IDapperPaymentRepository>();
			var payment = await paymentRepository.GetPaymentSummary(input);
			return PaymentProcessedNotificationProducer.Get(payment);
		}
	}
}
