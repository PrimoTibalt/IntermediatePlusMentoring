using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;

namespace NotificationsHandler.QueuesServices
{
	internal class PaymentCompleteSubscriberService(IChannelProvider channelProvider, IEnumerable<INotificationProvider> providers)
		: BaseQueueSubscriberService(channelProvider, providers)
	{
		protected override string QueueName => KnownQueueNames.PaymentCompleted;

		protected override string HeadingOfMessageBody => "The payment was completed.\n";
	}
}
