using Notifications.Infrastructure.Provider;
using RabbitMQ.Client;
using RegisterServicesSourceGenerator;

namespace Notifications.Infrastructure.Providers
{
	[RegisterService<IChannelProvider>(LifeTime.Singleton)]
	internal sealed class ChannelProvider(IConnectionProvider connectionProvider) : IChannelProvider
	{
		private readonly Lazy<Task<IChannel>> channel = new(async () =>
		{
			var connection = await connectionProvider.GetConnection();
			return await connection.CreateChannelAsync();
		});

		public async Task<IChannel> GetChannel()
		{
			return await channel.Value;
		}
	}
}