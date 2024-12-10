using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure.Models;
using RabbitMQ.Client;
using System.Text.Json.Serialization;

namespace Notifications.Infrastructure
{
	public static class ServiceCollectionsExtensions
	{
		public static void AddNotificationConnectionProvider(this IServiceCollection services, ConnectionFactory factory)
		{
			services.TryAddSingleton<ConnectionFactory>(services => factory);
			services.Register();
		}
	}

	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(Notification))]
	public partial class NotificationSerializationContext : JsonSerializerContext
	{ }
}
