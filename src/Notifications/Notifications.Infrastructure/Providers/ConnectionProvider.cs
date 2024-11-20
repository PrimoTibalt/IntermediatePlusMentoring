using RabbitMQ.Client;

namespace Notifications.Infrastructure.Provider
{
	internal class ConnectionProvider : IConnectionProvider
	{
		private readonly Lazy<Task<IConnection>> connection;

		public ConnectionProvider(ConnectionFactory factory)
		{
			connection = new(async () => await factory.CreateConnectionAsync());
		}

		public async Task<IConnection> GetConnection()
		{
			return await connection.Value;
		}
	}
}
