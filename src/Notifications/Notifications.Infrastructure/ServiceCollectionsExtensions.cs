﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure.Provider;
using Notifications.Infrastructure.Publishers;
using RabbitMQ.Client;

namespace Notifications.Infrastructure
{
	public static class ServiceCollectionsExtensions
	{
		public static void AddNotificationConnectionProvider(this IServiceCollection services, ConnectionFactory factory)
		{
			services.TryAddSingleton(factory);
			services.TryAddSingleton<IConnectionProvider, ConnectionProvider>();
			services.TryAddScoped<INotificationsPublisher, NotificationsPublisher>();
		}
	}
}