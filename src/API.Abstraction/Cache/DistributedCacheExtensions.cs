using Microsoft.Extensions.Caching.Distributed;

namespace API.Abstraction.Cache
{
	public static class DistributedCacheExtensions
	{
		public static DistributedCacheEntryOptions DefaultConfiguration => new()
		{
//		Better for Event resource but was not requested...
			SlidingExpiration = TimeSpan.FromSeconds(5),
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
		};

		public static async Task<TResult> GetOrCreate<TResult>(this IDistributedCache cache, string key, Func<Task<TResult>> action, CancellationToken token = default)
		{
			return await GetOrCreate(cache, key, action, null, token);
		}

		public static async Task<TResult> GetOrCreate<TResult>(this IDistributedCache cache, string key, Func<Task<TResult>> action, DistributedCacheEntryOptions options = null, CancellationToken token = default)
		{
			var cacheResult = CacheSerializationHelper.TryGetFromBytes<TResult>(await cache.GetAsync(key, token), out var result);
			if (cacheResult) return result;

			var actionResult = await action();
			var bytes = CacheSerializationHelper.GetBytes(actionResult);
			if (bytes is not null)
				await cache.SetAsync(key, bytes, options ?? DefaultConfiguration, token);

			return actionResult;
		}
	}
}
