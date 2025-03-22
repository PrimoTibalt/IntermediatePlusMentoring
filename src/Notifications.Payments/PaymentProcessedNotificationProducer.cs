using Entities.Payments;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Models;

namespace PaymentApplication.NotificationsSending
{
	public static class PaymentProcessedNotificationProducer
	{
		public static Notification Get(PaymentSummary payment)
		{
			var content = new Dictionary<string, string>()
			{
				{ "email", payment.UserEmail },
				{ NotificationContentKeys.PaymentId, payment.Id.ToString() }
			};

			var parameters = new Dictionary<string, string>()
			{
				{ "event", string.Join(",\n", payment.Events) },
				{ "amount", payment.Amount.ToString() }
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
