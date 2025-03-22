using DAL.Abstraction;
using Entities.Orders;
using Microsoft.EntityFrameworkCore;
using OrderApplication.Repository;

namespace DAL.Orders.Repository
{
	internal class CartItemRepository : GenericRepository<CartItem, long, OrderContext>, ICartItemRepository
	{
		public CartItemRepository(OrderContext context) : base(context) { }

		public Task<CartItem> GetBy(Guid cartId, int eventId, long seatId)
		{
			return _collection.Include(ci => ci.EventSeat)
				.ThenInclude(es => es.Seat)
				.ThenInclude(s => s.Row)
				.SingleOrDefaultAsync(ci => ci.CartId == cartId && ci.EventSeat.EventId == eventId && ci.EventSeatId == seatId);
		}
	}
}