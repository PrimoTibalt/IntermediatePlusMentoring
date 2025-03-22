using DAL.Abstraction;
using Entities.Orders;

namespace OrderApplication.Repository
{
	public interface ICartItemRepository : IGenericRepository<CartItem, long>
	{
		Task<CartItem> GetBy(Guid cartId, int eventId, long seatId);
	}
}