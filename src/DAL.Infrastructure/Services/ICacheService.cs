namespace Cache.Infrastructure.Services
{
	public interface ICacheService<TEntity>
	{
		Task CleanAsync(IList<TEntity> entities);
		string GetCacheKey(TEntity entity);
	}
}
