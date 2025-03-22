using Microsoft.Extensions.DependencyInjection;
using OrderApplication.Core;
using OrderApplication.Queries;

namespace OrderApplication
{
	public static class ServiceCollectionExtensions
	{
		public static void AddOrderApplication(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCartQuery).Assembly));
		}
	}
}