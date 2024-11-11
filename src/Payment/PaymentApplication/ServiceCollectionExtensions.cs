using DAL;
using DAL.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Infrastructure;
using PaymentApplication.Core;
using PaymentApplication.Handlers;
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
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPaymentHandler).Assembly));
		}
	}
}