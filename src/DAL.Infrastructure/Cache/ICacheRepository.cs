namespace OrderApplication.Cache
{
	public interface ICacheRepository
	{
		Task Delete(params string[] keys);
	}
}
