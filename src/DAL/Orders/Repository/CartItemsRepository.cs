
using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repository
{
    internal class CartItemRepository : GenericRepository<CartItem, long, OrderContext>, ICartItemRepository
    {
        public CartItemRepository(OrderContext context) : base(context) {}

        public Task<CartItem> GetBy(Guid cartId, int eventId, long seatId)
        {
            return _collection.SingleOrDefaultAsync(ci => ci.CartId == cartId && ci.EventSeat.EventId == eventId && ci.EventSeatId == seatId);
        }
    }
}