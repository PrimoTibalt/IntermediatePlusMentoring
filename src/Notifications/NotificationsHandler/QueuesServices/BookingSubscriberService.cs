using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;

namespace NotificationsHandler.QueuesServices
{
	internal class BookingSubscriberService(IChannelProvider channelProvider, IEnumerable<INotificationProvider> providers)
		: BaseQueueSubscriberService(channelProvider, providers)
	{
		protected override string QueueName => KnownQueueNames.Booking;

		protected override string HeadingOfMessageBody => "A payment was created.\n";
	}
}
