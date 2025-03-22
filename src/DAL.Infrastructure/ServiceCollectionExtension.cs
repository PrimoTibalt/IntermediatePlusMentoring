using DAL.Infrastructure.Cache;
using DAL.Infrastructure.Cache.Services;
using Entities.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderApplication.Cache;
using StackExchange.Redis;

namespace DAL.Infrastructure
{
	public static class ServiceCollectionExtension
	{
		public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			services.TryAddScoped<ICacheRepository, CacheRepository>();
			services.TryAddScoped<ICacheService<EventSeat>, EventSeatCacheService>();
			var cacheConfigurationOptions = new ConfigurationOptions
			{
				AbortOnConnectFail = true,
				EndPoints = { config.GetConnectionString("RedisConnection") }
			};
			services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(cacheConfigurationOptions));
			services.AddScoped(services => services.GetService<IConnectionMultiplexer>().GetDatabase());
		}
	}
}
