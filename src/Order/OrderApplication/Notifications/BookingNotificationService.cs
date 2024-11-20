using API.Abstraction.Notifications;
using DAL.Orders;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Publishers;
using Notifications.Infrastructure.Services;
using OrderApplication.Services;

namespace OrderApplication.Notifications
{
	public class BookingNotificationService(IPersistentNotificationPublisher publisher, ILogger<BookingNotificationService> logger)
		: NotificationService<(IList<CartItem> CartItems, long PaymentId)>(publisher, logger)
	{
		protected override Task<Notification> GetNotification((IList<CartItem> CartItems, long PaymentId) input)
		{
			return Task.FromResult(BookingNotificationProducer.Get(input.CartItems, input.PaymentId));
		}
	}
}
