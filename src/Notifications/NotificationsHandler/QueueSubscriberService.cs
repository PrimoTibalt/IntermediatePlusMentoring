using API.Abstraction.Notifications;
using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Provider;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationsHandler
{
	public class QueueSubscriberService : BackgroundService
	{
		private readonly IConnectionProvider _connectionProvider;

		public QueueSubscriberService(IConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var connection = await _connectionProvider.GetConnection();
			using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.ReceivedAsync += async (message, arguments) =>
			{
				var notification = Serializer.Deserialize<Notification>(arguments.Body);
				Console.WriteLine(notification.Operation);
			};

			await channel.BasicConsumeAsync(KnownQueueNames.Booking, true, consumer, cancellationToken: stoppingToken);

			await Task.CompletedTask;
		}
	}
}
