namespace Notifications.Infrastructure.Services
{
	public interface INotificationService<TInput>
	{
		Task SendNotification(TInput input);
	}
}
