using DAL;
using DAL.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApplication.Core;
using OrderApplication.Queries;

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

			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCartQuery).Assembly));
        }
    }
}