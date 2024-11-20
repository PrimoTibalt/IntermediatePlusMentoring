using DAL;
using DAL.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Services;
using PaymentApplication.Core;
using PaymentApplication.Handlers;
using PaymentApplication.Notifications;
using RabbitMQ.Client;

namespace PaymentApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddPaymentApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<PaymentContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddPaymentRepositories();
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			var factory = new ConnectionFactory
			{
				Uri = new(configuration.GetConnectionString("RabbitConnection"))
			};
			services.AddNotificationConnectionProvider(factory);
			services.TryAddScoped<INotificationService<long>, PaymentNotificationService>();
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPaymentHandler).Assembly));
		}
	}
}