namespace Notifications.Infrastructure
{
	public static class KnownOperationNotifications
	{
		public const string Booking = "Payment operation was created with id '{0}'.";
		public const string PaymentComplete = "The payment operation was completed.";
		public const string PaymentFailed = "The payment has failed.";
	}
}
