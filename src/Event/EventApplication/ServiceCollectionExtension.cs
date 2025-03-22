using EventApplication.Core;
using EventApplication.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace EventApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddEventApplication(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEventsHandler).Assembly));
		}
	}
}
