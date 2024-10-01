using Microsoft.EntityFrameworkCore;

namespace DAL.Order.Repositories
{
	sealed class CartRepository : ICartRepository
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

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			_context.Carts.Remove(entity);
		}

		public async Task<IList<Cart>> GetAll()
		{
			return await _context.Carts.ToListAsync();
		}

		[Obsolete("Use GetById(Guid)")]
		public async Task<Cart> GetById(int id)
		{
			throw new NotImplementedException("Use GetById(Guid).");
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
