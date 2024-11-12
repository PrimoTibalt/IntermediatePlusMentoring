namespace Notifications.Infrastructure
{
	public static class KnownQueueExchanges
	{
		public static IDictionary<string, string> Map => new Dictionary<string, string>
		{
			{ KnownQueueNames.Booking, "BookingExchange" },
			{ KnownQueueNames.PaymentCompleted, "PaymentCompleteExchange" },
			{ KnownQueueNames.PaymentFailed, "PaymentFailExchange" }
		};
	}
}
