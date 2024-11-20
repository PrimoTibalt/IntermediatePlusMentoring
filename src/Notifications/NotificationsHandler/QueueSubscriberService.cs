using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure.Providers;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Sockets;
using System.Text;

namespace NotificationsHandler
{
	public abstract class QueueSubscriberService : BackgroundService
	{
		private readonly IChannelProvider _channelProvider;
		protected readonly ResiliencePipeline resiliencePipeline;

		public QueueSubscriberService(IChannelProvider channelProvider)
		{
			_channelProvider = channelProvider;
			resiliencePipeline = new ResiliencePipelineBuilder()
				.AddRetry(new()
				{
					Delay = TimeSpan.FromSeconds(5),
					ShouldHandle = new PredicateBuilder()
						.Handle<SocketException>()
						.Handle<HttpRequestException>()
				})
				.Build();
		}

		protected abstract string QueueName { get; }
		protected abstract AsyncEventHandler<BasicDeliverEventArgs> AsyncEventHandler { get; }
		protected abstract IDictionary<string, string> knownParametersToContentStringsMap { get; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var channel = await _channelProvider.GetChannel();

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.ReceivedAsync += AsyncEventHandler;

			await channel.BasicConsumeAsync(QueueName, true, consumer, cancellationToken: stoppingToken);
		}

		protected virtual void AppendExistingParameters(StringBuilder body, IDictionary<string, string> parameters)
		{
			foreach (var (key, contentString) in knownParametersToContentStringsMap)
			{
				if (parameters.TryGetValue(key, out var value))
					body.AppendLine(string.Format(contentString, value));
			}
		}
	}
}
