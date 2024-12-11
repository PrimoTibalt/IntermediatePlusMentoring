using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Models;
using Notifications.Infrastructure.Publishers;

namespace Notifications.Infrastructure.Services
{
	public abstract class NotificationService<TInput>(IPersistentNotificationPublisher publisher, ILogger logger)
		: INotificationService<TInput>
	{
		private readonly IPersistentNotificationPublisher _publisher = publisher;
		private readonly ILogger _logger = logger;

		public Task SendNotification(TInput input)
		{
			_ = Task.Run(async () =>
			{
				await SendRequest(input);
			});

			return Task.CompletedTask;
		}

		private async Task SendRequest(TInput input)
		{
			try
			{
				var notification = await GetNotification(input);
				await _publisher.PersistentPublish(notification, KnownQueueNames.Notifications);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Notification for payment was not sent.");
			}
		}

		protected abstract Task<Notification> GetNotification(TInput input);
	}
}
