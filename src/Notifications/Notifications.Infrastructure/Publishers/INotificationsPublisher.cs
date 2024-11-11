namespace Notifications.Infrastructure.Publishers
{
	public interface INotificationsPublisher
	{
		Task SendProtoSerializedMessage<T>(T message, string queue);
		Task SendProtoSerializedMessage<T>(T message, string queue, string routingKey);
	}
}
