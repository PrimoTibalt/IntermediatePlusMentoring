using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;

namespace NotificationsHandler.QueuesServices
{
	internal class PaymentFailSubscriberService(IChannelProvider channelProvider, IEnumerable<INotificationProvider> providers)
		: BaseQueueSubscriberService(channelProvider, providers)
	{
		protected override string QueueName => KnownQueueNames.PaymentFailed;

		protected override string HeadingOfMessageBody => "A payment has failed.\n";
	}
}
