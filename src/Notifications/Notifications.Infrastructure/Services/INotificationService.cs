namespace OrderApplication.Notifications
{
	public interface INotificationService<TInput>
	{
		Task SendNotification(TInput input);
	}
}
