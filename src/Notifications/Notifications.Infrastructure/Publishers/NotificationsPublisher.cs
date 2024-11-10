using Notifications.Infrastructure.Provider;
using ProtoBuf;
using RabbitMQ.Client;

namespace Notifications.Infrastructure.Publishers
{
	internal class NotificationsPublisher(IConnectionProvider connectionProvider) : INotificationsPublisher
	{
		private readonly IConnectionProvider _connectionProvider = connectionProvider;

		public async Task SendMessage<T>(T message, string queue)
		{
			await SendMessage(message, queue, queue);
		}

		public async Task SendMessage<T>(T message, string queue, string routingKey)
		{
			const string exchangeName = "BasicExchange";
			var connection = await _connectionProvider.GetConnection();
			using var channel = await connection.CreateChannelAsync();
			await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
			await channel.QueueDeclareAsync(queue, false, false, false);

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, message);

			var props = new BasicProperties
			{
				DeliveryMode = DeliveryModes.Persistent
			};
			await channel.BasicPublishAsync(exchangeName, routingKey, true, props, stream.ToArray());
		}
	}
}
