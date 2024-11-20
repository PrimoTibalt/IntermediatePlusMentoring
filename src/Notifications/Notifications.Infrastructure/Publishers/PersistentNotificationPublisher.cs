using API.Abstraction.Notifications;
using DAL;
using DAL.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Notifications.Infrastructure.Publishers
{
	internal class PersistentNotificationPublisher(INotificationsPublisher publisher,
		IServiceScopeFactory scopeFactory,
		ILogger<PersistentNotificationPublisher> logger)
		: IPersistentNotificationPublisher
	{
		private readonly INotificationsPublisher _publisher = publisher;
		private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
		private readonly ILogger _logger = logger;

		public async Task PersistentPublish(Notification notification, string queue)
		{
			_ = notification ?? throw new ArgumentNullException(nameof(notification));

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, notification);

			var notificationEntity = new NotificationEntity
			{
				Id = notification.Id,
				Timestamp = notification.Timestamp,
				Status = (int)NotificationStatus.InProgress,
				Data = stream.ToArray()
			};
			try
			{
				using var scope = _scopeFactory.CreateScope();
				var notificationEntityRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<NotificationEntity, Guid>>();
				await notificationEntityRepository.Create(notificationEntity);
				await notificationEntityRepository.Save();
				await _publisher.SendMessage(notificationEntity.Data, queue);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to create persistent notification.");
			}
		}
	}
}
