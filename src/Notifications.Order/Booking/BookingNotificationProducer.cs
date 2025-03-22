using Entities.Orders;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Models;

namespace Notifications.Order.Booking
{
	internal static class BookingNotificationProducer
	{
		public static Notification Get(IList<CartItem> cartItems, long paymentId)
		{
			var content = new Dictionary<string, string>
			{
				{ NotificationContentKeys.Email, $"anton.shheglov.{paymentId}@gmail.com" },
				{ NotificationContentKeys.PaymentId, paymentId.ToString() }
			};

			var parameters = new Dictionary<string, string>
			{
				{ "amount", cartItems.Sum(ci => ci.Price?.Sum ?? ci.EventSeat.Price?.Sum).ToString() },
			};

			return new()
			{
				Id = Guid.NewGuid(),
				Content = content,
				Parameters = parameters,
				Operation = string.Format(KnownOperationNotifications.Booking, paymentId),
				Subject = "A payment was created"
			};
		}
	}
}
