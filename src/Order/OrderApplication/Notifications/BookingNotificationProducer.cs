using API.Abstraction.Notifications;
using DAL.Orders;

namespace OrderApplication.Services
{
	internal static class BookingNotificationProducer
	{
		public static Notification Get(IList<CartItem> cartItems, long paymentId)
		{
			var content = new Dictionary<string, string>
			{
				{ "email", $"anton.shheglov.{paymentId}@gmail.com" }
			};

			var parameters = new Dictionary<string, string>
			{
				{ "paymentId", paymentId.ToString() },
				{ "amount", cartItems.Sum(ci => ci.Price?.Sum ?? ci.EventSeat.Price?.Sum).ToString() }
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
