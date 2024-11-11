using API.Abstraction.Notifications;
using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Provider;
using Notifications.Infrastructure.Providers;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationsHandler
{
	public class QueueSubscriberService(IChannelProvider channelProvider) : BackgroundService
	{
		private readonly IChannelProvider _channelProvider = channelProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var channel = await _channelProvider.GetChannel();

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.ReceivedAsync += async (message, arguments) =>
			{
				var notification = Serializer.Deserialize<Notification>(arguments.Body);
				Console.WriteLine(notification.Operation);
				Console.WriteLine(notification.Timestamp);
				await Task.CompletedTask;
			};

			await channel.BasicConsumeAsync(KnownQueueNames.Booking, true, consumer, cancellationToken: stoppingToken);
		}
	}
}
