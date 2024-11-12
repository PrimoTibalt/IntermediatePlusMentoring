using API.Abstraction.Notifications;
using DAL.Orders;
using Notifications.Infrastructure.Models;

namespace OrderApplication.Services
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
				Operation = "booking"
			};
		}
	}
}
