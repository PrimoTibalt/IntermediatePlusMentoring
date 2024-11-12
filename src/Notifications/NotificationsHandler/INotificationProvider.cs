namespace NotificationsHandler
{
	public interface INotificationProvider
	{
		Task Send(Message message);
	}
}
