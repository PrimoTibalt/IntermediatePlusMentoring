using DAL;
using DAL.Notifications;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;

namespace NotificationsHandler.QueuesServices
{
	internal class BookingSubscriberService(IChannelProvider channelProvider, IEnumerable<INotificationProvider> providers,
		IGenericRepository<NotificationEntity, Guid> repository)
		: BaseQueueSubscriberService(channelProvider, providers, repository)
	{
		protected override string QueueName => KnownQueueNames.Booking;

		protected override string HeadingOfMessageBody => "A payment was created.\n";
	}
}
