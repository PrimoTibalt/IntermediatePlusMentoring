using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Models;
using Notifications.Infrastructure.Publishers;
using Notifications.Infrastructure.Services;
using OrderApplication.Repository;

namespace Notifications.Order.Booking
{
	internal sealed class BookingNotificationService(IPersistentNotificationPublisher publisher, ILogger<BookingNotificationService> logger, IServiceScopeFactory scopeFactory)
		: NotificationService<long>(publisher, logger)
	{
		protected override async Task<Notification> GetNotification(long input)
		{
			await using var scope = scopeFactory.CreateAsyncScope();
			var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
			var payment = await paymentRepository.GetPaymentWithRelatedInfo(input);
			return BookingNotificationProducer.Get([.. payment.Cart.CartItems], input);
		}
	}
}
