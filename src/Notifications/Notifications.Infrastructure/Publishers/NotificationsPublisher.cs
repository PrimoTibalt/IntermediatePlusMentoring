using Notifications.Infrastructure.Provider;
using RabbitMQ.Client;

namespace Notifications.Infrastructure.Publishers
{
	internal class NotificationsPublisher(IConnectionProvider connectionProvider) : INotificationsPublisher
	{
		private readonly IConnectionProvider _connectionProvider = connectionProvider;

		public async Task SendMessage(byte[] message, string queue)
		{
			await SendMessage(message, queue, queue, KnownQueueExchanges.Map[queue]);
		}

		private async Task SendMessage(byte[] message, string queue, string routingKey, string exchange)
		{
			var connection = await _connectionProvider.GetConnection();
			using var channel = await connection.CreateChannelAsync();

			var props = new BasicProperties
			{
				DeliveryMode = DeliveryModes.Persistent
			};
			await channel.BasicPublishAsync(exchange, routingKey, true, props, message);
		}
	}
}
