namespace Cache.Infrastructure
{
	public interface ICacheRepository
	{
		Task Delete(params string[] keys);
	}
}
