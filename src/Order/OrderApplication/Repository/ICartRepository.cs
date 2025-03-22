using DAL.Abstraction;
using Entities.Orders;
using System.Data;

namespace OrderApplication.Repository
{
	public interface ICartRepository : IGenericRepository<Cart, Guid>
	{
		Task BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);
		Task CommitTransaction();
		Task<IList<CartItem>> GetItemsFull(Guid id);
		Task<IList<CartItem>> GetItemsWithEventSeat(Guid id);
	}
}