using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Payments
{
	public static class ServiceCollectionExtensions
	{
		public static void AddPaymentNotifications(this IServiceCollection services)
		{
			services.Register();
		}
	}
}
