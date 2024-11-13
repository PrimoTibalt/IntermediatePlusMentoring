namespace Notifications.Infrastructure.Publishers
{
	public interface INotificationsPublisher
	{
		Task SendMessage(byte[] message, string queue);
	}
}
