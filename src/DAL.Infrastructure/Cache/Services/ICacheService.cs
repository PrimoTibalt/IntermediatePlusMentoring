namespace DAL.Infrastructure.Cache.Services
{
	public interface ICacheService<TEntity>
	{
		Task Clean(IList<TEntity> entities);
		Task Clean(Guid id);
		string GetCacheKey(TEntity entity);
	}
}
