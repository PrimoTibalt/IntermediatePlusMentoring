using Notifications.Infrastructure.Provider;
using ProtoBuf;
using RabbitMQ.Client;

namespace Notifications.Infrastructure.Publishers
{
	internal class NotificationsPublisher(IConnectionProvider connectionProvider) : INotificationsPublisher
	{
		private readonly IConnectionProvider _connectionProvider = connectionProvider;

		public async Task SendProtoSerializedMessage<T>(T message, string queue)
		{
			await SendProtoSerializedMessage(message, queue, queue, KnownQueueExchanges.Map[queue]);
		}

		public async Task SendProtoSerializedMessage<T>(T message, string queue, string routingKey, string exchange)
		{
			var connection = await _connectionProvider.GetConnection();
			using var channel = await connection.CreateChannelAsync();

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, message);

			var props = new BasicProperties
			{
				DeliveryMode = DeliveryModes.Persistent
			};
			await channel.BasicPublishAsync(exchange, routingKey, true, props, stream.ToArray());
		}
	}
}
