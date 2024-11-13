using DAL;
using DAL.Notifications;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;

namespace NotificationsHandler.QueuesServices
{
	internal class PaymentFailSubscriberService(IChannelProvider channelProvider, IEnumerable<INotificationProvider> providers,
		IGenericRepository<NotificationEntity, Guid> repository)
		: BaseQueueSubscriberService(channelProvider, providers, repository)
	{
		protected override string QueueName => KnownQueueNames.PaymentFailed;

		protected override string HeadingOfMessageBody => "A payment has failed.\n";
	}
}
