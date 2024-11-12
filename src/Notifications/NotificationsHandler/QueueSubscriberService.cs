using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure.Providers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationsHandler
{
	public abstract class QueueSubscriberService(IChannelProvider channelProvider) : BackgroundService
	{
		private readonly IChannelProvider _channelProvider = channelProvider;
		protected abstract string QueueName { get; }
		protected abstract AsyncEventHandler<BasicDeliverEventArgs> AsyncEventHandler { get; }
		protected abstract IDictionary<string, string> knownBookingParameterToContentStringsMap { get; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var channel = await _channelProvider.GetChannel();

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.ReceivedAsync += AsyncEventHandler;
			await channel.BasicConsumeAsync(QueueName, true, consumer, cancellationToken: stoppingToken);
		}

		protected virtual void AppendExistingParameters(StringBuilder body, IDictionary<string, string> parameters)
		{
			foreach (var (key, contentString) in knownBookingParameterToContentStringsMap)
			{
				if (parameters.TryGetValue(key, out var value))
					body.AppendLine(string.Format(contentString, value));
			}
		}
	}
}
