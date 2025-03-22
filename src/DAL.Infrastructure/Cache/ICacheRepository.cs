namespace DAL.Infrastructure.Cache
{
	public interface ICacheRepository
	{
		Task Delete(params string[] keys);
	}
}
