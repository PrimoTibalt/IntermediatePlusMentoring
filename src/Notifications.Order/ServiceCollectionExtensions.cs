using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Services;
using Notifications.Order.Booking;
using RabbitMQ.Client;

namespace Notifications.Order
{
	public static class ServiceCollectionExtensions
	{
		public static void AddBookingNotificationService(this IServiceCollection services, IConfiguration config)
		{
			var factory = new ConnectionFactory
			{
				Uri = new(config.GetConnectionString("RabbitConnection"))
			};
			services.AddNotificationConnectionProvider(factory);
			services.TryAddScoped<INotificationService<long>, BookingNotificationService>();
		}
	}
}
