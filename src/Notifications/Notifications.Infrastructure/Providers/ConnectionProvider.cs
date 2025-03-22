using RabbitMQ.Client;
using RegisterServicesSourceGenerator;

namespace Notifications.Infrastructure.Provider
{
	[RegisterService<IConnectionProvider>(LifeTime.Singleton)]
	internal sealed class ConnectionProvider : IConnectionProvider
	{
		private Lazy<Task<IConnection>> connection;
		private readonly ConnectionFactory _factory;

		public ConnectionProvider(ConnectionFactory factory)
		{
			_factory = factory;
			connection = GetNewConnection();
		}

		public async Task<IConnection> GetConnection()
		{
			try
			{
				return await connection.Value;
			}
			catch
			{
				connection = GetNewConnection();
				await Task.Delay(1000);
				return await connection.Value;
			}
		}

		private Lazy<Task<IConnection>> GetNewConnection() =>
			new(async () => await _factory.CreateConnectionAsync());
	}
}
