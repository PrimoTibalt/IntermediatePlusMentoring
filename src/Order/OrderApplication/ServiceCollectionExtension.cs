using DAL;
using DAL.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderApplication
{
    public static class ServiceCollectionExtension
    {
        public static void AddOrderApplication(this IServiceCollection services, IConfiguration configuration)
        {
			// services.AddAutoMapper(typeof().Assembly);
			services.AddDbContext<OrderContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddOrderRepositories();

			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Carts.Items).Assembly));
        }
    }
}