namespace Notifications.Infrastructure.Publishers
{
	public interface INotificationsPublisher
	{
		Task SendMessage<T>(T message, string queue);
		Task SendMessage<T>(T message, string queue, string routingKey);
	}
}
