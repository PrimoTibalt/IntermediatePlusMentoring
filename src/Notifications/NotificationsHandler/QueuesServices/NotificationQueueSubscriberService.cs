using API.Abstraction.Notifications;
using DAL;
using DAL.Notifications;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Providers;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationsHandler.QueuesServices
{
	internal class NotificationQueueSubscriberService(IChannelProvider channelProvider,
		IEnumerable<INotificationProvider> providers,
		IGenericRepository<NotificationEntity, Guid> repository,
		ILogger<NotificationQueueSubscriberService> logger) : QueueSubscriberService(channelProvider)
	{
		private readonly IGenericRepository<NotificationEntity, Guid> _repository = repository;
		private readonly ILogger _logger = logger;
		protected readonly IEnumerable<INotificationProvider> _providers = providers;

		protected override IDictionary<string, string> knownParametersToContentStringsMap => new Dictionary<string, string>()
		{
			{ "amount", "Total amount is {0}$." },
			{ "event", "Contains tickets to events {0}." }
		};

		protected override AsyncEventHandler<BasicDeliverEventArgs> AsyncEventHandler => ProcessMessage;

		protected override string QueueName => KnownQueueNames.Notifications;

		private async Task ProcessMessage(object message, BasicDeliverEventArgs ea)
		{
			using var stream = new MemoryStream(ea.Body.ToArray(), false);
			var notification = JsonSerializer.Deserialize<Notification>(stream);
			var body = new StringBuilder(notification.Operation);
			AppendExistingParameters(body, notification.Parameters);
			body.AppendLine($"Created at {notification.Timestamp.ToString("dd-MMM-yyyy H:mm")}.");
			var entity = new Message
			{
				To = "anton.shheglov.1@gmail.com", // notification.Content[NotificationContentKeys.Email],
				Subject = notification.Subject,
				Body = body.ToString()
			};

			var anySent = await Send(entity);
			var status = anySent ? NotificationStatus.Completed : NotificationStatus.Failed;
			await UpdatePersistenceEntity(notification.Id, status);
		}

		private async Task<bool> Send(Message message)
		{
			var anySent = false;
			foreach (var provider in _providers)
			{
				try
				{
					await resiliencePipeline.ExecuteAsync(async (token) =>
					{
						await provider.Send(message);
					});
					anySent = true;
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Failed to send notification using '{providerType}' provider.", new { providerType = provider.GetType() });
				}
			}

			return anySent;
		}

		private async Task UpdatePersistenceEntity(Guid notificationId, NotificationStatus status)
		{
			try
			{
				var persistentEntity = await _repository.GetById(notificationId);
				persistentEntity.Status = (int) status;
				persistentEntity.Timestamp = DateTime.UtcNow;
				_repository.Update(persistentEntity);
				await _repository.Save();
			}
			catch
			{
				Console.WriteLine($"Failed to update notification '{notificationId}' to status {status}.");
			}
		}
	}
}
