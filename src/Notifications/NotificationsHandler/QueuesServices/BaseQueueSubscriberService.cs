using API.Abstraction.Notifications;
using DAL;
using DAL.Notifications;
using Notifications.Infrastructure.Providers;
using ProtoBuf;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationsHandler.QueuesServices
{
	internal abstract class BaseQueueSubscriberService(IChannelProvider channelProvider,
		IEnumerable<INotificationProvider> providers,
		IGenericRepository<NotificationEntity, Guid> repository) : QueueSubscriberService(channelProvider)
	{
		protected readonly IEnumerable<INotificationProvider> _providers = providers;
		private readonly IGenericRepository<NotificationEntity, Guid> _repository = repository;

		protected override IDictionary<string, string> knownBookingParameterToContentStringsMap => new Dictionary<string, string>()
		{
			{ "amount", "Total amount is {0}$." },
			{ "event", "Contains tickets to events {0}." }
		};

		protected override AsyncEventHandler<BasicDeliverEventArgs> AsyncEventHandler => ProcessMessage;

		protected abstract string HeadingOfMessageBody { get; }

		private async Task ProcessMessage(object message, BasicDeliverEventArgs ea)
		{
			var notification = Serializer.Deserialize<Notification>(ea.Body);
			var body = new StringBuilder(HeadingOfMessageBody);
			AppendExistingParameters(body, notification.Parameters);
			body.AppendLine($"Created at {notification.Timestamp.ToString("dd-MMM-yyyy H:mm")}");
			var entity = new Message
			{
				To = "anton.shheglov.1@gmail.com", // notification.Content[NotificationContentKeys.Email],
				Subject = notification.Operation,
				Body = body.ToString()
			};

			var anySent = false;
			foreach (var provider in _providers)
			{
				try
				{
					await provider.Send(entity);
					anySent = true;
				}
				catch
				{
					Console.WriteLine($"Failed to send notification using '{provider.GetType()}' provider.");
				}
			}

			var status = anySent ? NotificationStatus.Completed : NotificationStatus.Failed;
			try
			{
				var persistentEntity = await _repository.GetById(notification.Id);
				persistentEntity.Status = (int) status;
				persistentEntity.Timestamp = DateTime.UtcNow;
				_repository.Update(persistentEntity);
				await _repository.Save();
			}
			catch
			{
				Console.WriteLine($"Failed to update notification '{notification.Id}' to status {status}.");
			}
		}
	}
}
