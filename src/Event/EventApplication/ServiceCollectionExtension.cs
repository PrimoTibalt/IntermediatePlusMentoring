using DAL;
using DAL.Events;
using EventApplication.Core;
using EventApplication.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddEventApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddDbContextPool<EventContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddEventsRepositories();

			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEventsHandler).Assembly));
		}
	}
}
