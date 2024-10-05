using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repositories
{
	sealed class CartRepository : IGenericRepository<Cart, Guid>
	{
		private readonly OrderContext _context;

		public CartRepository(OrderContext context)
		{
			_context = context;
		}

		public void Create(Cart entity)
		{
			_context.Carts.Add(entity);
		}

		public async Task Delete(Guid id)
		{
			var entity = await GetById(id);
			_context.Carts.Remove(entity);
		}

		public async Task<IList<Cart>> GetAll()
		{
			return await _context.Carts.ToListAsync();
		}

		public async Task<Cart> GetById(Guid id)
		{
			return await _context.Carts.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Cart entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
