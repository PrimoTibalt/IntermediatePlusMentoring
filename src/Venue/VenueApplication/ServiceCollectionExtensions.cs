using Microsoft.Extensions.DependencyInjection;
using VenueApplication.Core;
using VenueApplication.Handlers;

namespace VenueApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddVenueApplicaiton(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllVenuesHandler).Assembly));
		}
	}
}
