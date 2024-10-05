using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repositories
{
	sealed class UserRepository : IGenericRepository<User, int>
	{
		private readonly OrderContext _context;

		public UserRepository(OrderContext context)
		{
			_context = context;
		}

		public void Create(User entity)
		{
			_context.Users.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);

			_context.Users.Remove(entity);
		}

		public async Task<IList<User>> GetAll()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User> GetById(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(User entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
