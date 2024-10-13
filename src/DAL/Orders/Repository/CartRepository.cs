using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repository
{
    internal class CartRepository : GenericRepository<Cart, Guid, OrderContext>, ICartRepository
    {
        public CartRepository(OrderContext context) : base(context) {}

        public async Task<IList<CartItem>> GetItems(Guid id)
        {
           var cart = await _collection.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == id); 
           return [.. cart.CartItems];
        }
    }
}