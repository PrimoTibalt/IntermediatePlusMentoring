using Cache.Infrastructure.Services;
using Entities.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Cache.Infrastructure
{
	public static class ServiceCollectionExtension
	{
		public static void AddCaching(this IServiceCollection services, IConfiguration config)
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
