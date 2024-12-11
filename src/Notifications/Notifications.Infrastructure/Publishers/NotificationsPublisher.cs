using Notifications.Infrastructure.Provider;
using RabbitMQ.Client;
using RegisterServicesSourceGenerator;

namespace Notifications.Infrastructure.Publishers
{
	[RegisterService<INotificationsPublisher>(LifeTime.Scoped)]
	internal class NotificationsPublisher(IConnectionProvider connectionProvider) : INotificationsPublisher
	{
		private readonly IConnectionProvider _connectionProvider = connectionProvider;

		public async Task SendMessage(byte[] message, string queue)
		{
			await SendMessage(message, queue, KnownQueueExchanges.Map[queue]);
		}

		private async Task SendMessage(byte[] message, string routingKey, string exchange)
		{
			var connection = await _connectionProvider.GetConnection();
			using var channel = await connection.CreateChannelAsync();

			var props = new BasicProperties();
			await channel.BasicPublishAsync(exchange, routingKey, true, props, message);
		}
	}
}
