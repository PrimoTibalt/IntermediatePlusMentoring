using API.Abstraction.Notifications;
using DAL;
using DAL.Notifications;
using ProtoBuf;

namespace Notifications.Infrastructure.Publishers
{
	internal class PersistentNotificationPublisher(INotificationsPublisher publisher, IGenericRepository<NotificationEntity, Guid> notificationEntityRepository)
		: IPersistentNotificationPublisher
	{
		private readonly INotificationsPublisher _publisher = publisher;

		private readonly IGenericRepository<NotificationEntity, Guid> _notificationEntityRepository = notificationEntityRepository;

		public async Task PersistentPublish(Notification notification, string queue)
		{
			_ = notification ?? throw new ArgumentNullException(nameof(notification));

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, notification);

			var notificationEntity = new NotificationEntity
			{
				Id = notification.Id,
				Timestamp = notification.Timestamp,
				Status = (int) NotificationStatus.InProgress,
				Data = stream.ToArray()
			};
			try
			{
				await _notificationEntityRepository.Create(notificationEntity);
				await _notificationEntityRepository.Save();
				await _publisher.SendMessage(notificationEntity.Data, queue);
			}
			catch
			{
				// logging
			}
		}
	}
}
