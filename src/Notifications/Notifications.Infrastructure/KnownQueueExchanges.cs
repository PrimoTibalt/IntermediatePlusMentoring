namespace Notifications.Infrastructure
{
	public static class KnownQueueExchanges
	{
		public static IDictionary<string, string> Map => new Dictionary<string, string>
		{
			{ KnownQueueNames.Notifications, string.Empty },
		};
	}
}
