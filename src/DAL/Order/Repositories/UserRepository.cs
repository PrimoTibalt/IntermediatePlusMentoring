using Microsoft.EntityFrameworkCore;

namespace DAL.Order.Repositories
{
	sealed class UserRepository : IGenericRepository<User>
	{
		private readonly OrderContext _context;

		public UserRepository(OrderContext context)
		{
			_context = context;
		}

		public void Create(User entity)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IList<User>> GetAll()
		{
			throw new NotImplementedException();
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
