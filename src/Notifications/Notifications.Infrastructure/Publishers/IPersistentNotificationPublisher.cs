using API.Abstraction.Notifications;

namespace Notifications.Infrastructure.Publishers
{
	public interface IPersistentNotificationPublisher
	{
		Task PersistentPublish(Notification notification, string queue);
	}
}
