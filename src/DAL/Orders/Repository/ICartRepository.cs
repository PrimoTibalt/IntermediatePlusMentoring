namespace DAL.Orders.Repository
{
	public interface ICartRepository : IGenericRepository<Cart, Guid>
	{
		Task BeginTransaction();
		Task CommitTransaction();
		Task RollbackTransaction();
		Task<IList<CartItem>> GetItemsFull(Guid id);
		Task<IList<CartItem>> GetItemsWithEventSeat(Guid id);
		IQueryable<CartItem> GetItemsWithEventSeatQueryable(Guid id);
	}
}