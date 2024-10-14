using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repository
{
    internal class CartRepository : GenericRepository<Cart, Guid, OrderContext>, ICartRepository
    {
        public CartRepository(OrderContext context) : base(context) {}

        public async Task<IList<CartItem>> GetItemsFull(Guid id)
        {
           var cart = await _collection.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Price)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.EventSeat)
                .FirstOrDefaultAsync(c => c.Id == id); 
           if (cart is null) return null;
           return [.. cart.CartItems];
        }

        public async Task<IList<CartItem>> GetItemsWithEventSeat(Guid id)
        {
            var cart = await _collection.Include(c => c.CartItems).ThenInclude(ci => ci.EventSeat).FirstOrDefaultAsync(c => c.Id == id);
            if (cart is null) return null;
            return [.. cart.CartItems];
        }
    }
}