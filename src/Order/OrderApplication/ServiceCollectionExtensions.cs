using DAL;
using DAL.Orders;
using DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApplication.Core;
using OrderApplication.Queries;
using Notifications.Infrastructure;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure.Services;
using OrderApplication.Notifications;

namespace OrderApplication
{
	public static class ServiceCollectionExtensions
	{
		public static void AddOrderApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddDbContext<OrderContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddOrderRepositories();
			services.AddInfrastructure();
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCartQuery).Assembly));
			var factory = new ConnectionFactory
			{
				Uri = new(configuration.GetConnectionString("RabbitConnection"))
			};
			services.AddNotificationConnectionProvider(factory);
			services.TryAddScoped<INotificationService<(IList<CartItem> CartItems, long PaymentId)>, BookingNotificationService>();
		}
	}
}