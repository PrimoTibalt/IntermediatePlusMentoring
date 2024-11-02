using StackExchange.Redis;

namespace OrderApplication.Cache
{
	internal class CacheRepository(IDatabase _database) : ICacheRepository
	{
		public async Task Delete(params string[] keys)
		{
			await _database.KeyDeleteAsync(keys.Select(key => new RedisKey(key)).ToArray());
		}
	}
}
