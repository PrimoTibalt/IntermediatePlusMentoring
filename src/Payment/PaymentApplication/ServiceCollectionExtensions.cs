using DAL;
using DAL.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentApplication.Core;
using PaymentApplication.Handlers;

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
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPaymentHandler).Assembly));
		}
	}
}