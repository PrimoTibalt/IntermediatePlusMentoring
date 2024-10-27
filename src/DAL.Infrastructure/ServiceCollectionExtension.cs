using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderApplication.Cache;

namespace DAL.Infrastructure
{
	public static class ServiceCollectionExtension
	{
		public static void AddInfrastructure(this IServiceCollection services)
		{
			services.TryAddScoped<ICacheCleaner, CacheCleaner>();
		}
	}
}
