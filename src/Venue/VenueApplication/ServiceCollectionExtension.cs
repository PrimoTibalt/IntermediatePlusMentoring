using DAL;
using DAL.Venues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VenueApplication.Core;
using VenueApplication.Entities;

namespace VenueApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddVenueApplicaiton(this IServiceCollection services, IConfiguration config)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddDbContext<VenueContext>(options =>
			{
				options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
			});
			services.AddVenuesRepositories();

			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VenueDetails).Assembly));
		}
	}
}
