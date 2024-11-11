using API.Abstraction.Notifications;

namespace PaymentApplication.Notifications
{
	public static class PaymentProcessedNotificationProducer
	{
		public static Notification Get(long paymentId, bool complete)
		{
			var content = new Dictionary<string, string>()
			{
				{ "email", $"anton.shheglov.{paymentId}@gmail.com" }
			};

			var parameters = new Dictionary<string, string>()
			{
				{ "paymentId", paymentId.ToString() },
			};

			return new()
			{
				Id = Guid.NewGuid(),
				Content = content,
				Parameters = parameters,
				Operation = complete ? "payment operation was completed" : "payment operation was failed"
			};
		}
	}
}
