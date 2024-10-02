using Microsoft.EntityFrameworkCore;

namespace DAL.Order.Repositories
{
	sealed class CartItemRepository : IGenericRepository<CartItem, long>
	{
		private readonly OrderContext _context;

		public CartItemRepository(OrderContext context)
		{
			_context = context;
		}

		public void Create(CartItem entity)
		{
			_context.CartItems.Add(entity);
		}

		public async Task Delete(long id)
		{
			var entity = await GetById(id);
			_context.CartItems.Remove(entity);
		}

		public async Task<IList<CartItem>> GetAll()
		{
			return await _context.CartItems.ToListAsync();
		}

		public async Task<CartItem> GetById(long id)
		{
			return await _context.CartItems.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(CartItem entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
