namespace DAL.Infrastructure.Cache.Services
{
	public interface ICacheService<TEntity>
	{
		Task Clean(IList<TEntity> entities);
		string GetCacheKey(TEntity entity);
	}
}
