using API.Abstraction.Notifications;
using DAL.Payments;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Models;

namespace PaymentApplication.Notifications
{
	public static class PaymentProcessedNotificationProducer
	{
		public static Notification Get(Payment payment)
		{
			var content = new Dictionary<string, string>()
			{
				{ "email", payment.Cart.User.Email },
				{ NotificationContentKeys.PaymentId, payment.Id.ToString() }
			};

			var events = new HashSet<string>(payment.Cart.CartItems.Select(ci => ci.EventSeat.Event.Name));
			var amount = payment.Cart.CartItems.Sum(ci => ci.Price.Sum);
			var parameters = new Dictionary<string, string>()
			{
				{ "event", string.Join(",\n", events) },
				{ "amount", amount.ToString() }
			};

			var complete = payment.Status == (int)PaymentStatus.Completed;
			return new()
			{
				Id = Guid.NewGuid(),
				Content = content,
				Parameters = parameters,
				Subject = complete ? "payment operation was completed" : "payment operation was failed",
				Operation = complete ? KnownOperationNotifications.PaymentComplete : KnownOperationNotifications.PaymentFailed,
			};
		}
	}
}
