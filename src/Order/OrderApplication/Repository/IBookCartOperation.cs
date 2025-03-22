namespace OrderApplication.Repository
{
	public interface IBookCartOperation
	{
		Task<bool> TryBookCart(Guid id, bool optimistic);
	}
}
