using API.Abstraction.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure.Provider;
using Notifications.Infrastructure.Providers;
using Notifications.Infrastructure.Publishers;
using RabbitMQ.Client;
using System.Text.Json.Serialization;

namespace Notifications.Infrastructure
{
	public static class ServiceCollectionsExtensions
	{
		public static void AddNotificationConnectionProvider(this IServiceCollection services, ConnectionFactory factory)
		{
			services.TryAddSingleton<ConnectionFactory>(services => factory);
			services.TryAddSingleton<IConnectionProvider, ConnectionProvider>();
			services.TryAddSingleton<IChannelProvider, ChannelProvider>();
			services.TryAddScoped<INotificationsPublisher, NotificationsPublisher>();
			services.TryAddScoped<IPersistentNotificationPublisher, PersistentNotificationPublisher>();
		}
	}

	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(Notification))]
	public partial class NotificationSerializationContext : JsonSerializerContext
	{ }
}
