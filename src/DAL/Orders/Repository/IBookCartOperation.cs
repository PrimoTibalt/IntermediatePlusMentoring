namespace DAL.Orders.Repository
{
	public interface IBookCartOperation
	{
		Task<bool> TryBookCart(Guid id, bool optimistic);
	}
}
