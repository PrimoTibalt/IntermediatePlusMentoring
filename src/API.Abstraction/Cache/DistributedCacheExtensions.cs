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
			var cacheResult = await cache.GetAsync(key, token);
			if (cacheResult is not null)
			{
				if (CacheSerializationHelper.TryGetFromBytes<TResult>(cacheResult, out var result))
					return result;

				throw new InvalidOperationException($"Can't deserialize cache entry with cache key '{key}'.");
			}

			var actionResult = await action();
			if (actionResult is not null)
			{
				var bytes = CacheSerializationHelper.GetBytes(actionResult);
				await cache.SetAsync(key, bytes, options ?? DefaultConfiguration, token);
			}

			return actionResult;
		}
	}
}
