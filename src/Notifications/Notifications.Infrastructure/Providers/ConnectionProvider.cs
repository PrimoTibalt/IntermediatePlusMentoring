using RabbitMQ.Client;
using RegisterServicesSourceGenerator;

namespace Notifications.Infrastructure.Provider
{
	[RegisterService<IConnectionProvider>(LifeTime.Singleton)]
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
