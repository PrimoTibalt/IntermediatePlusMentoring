using Microsoft.EntityFrameworkCore;

namespace DAL.Payments.Repositories
{
	sealed class PaymentRepository : IGenericRepository<Payment, long>
	{
		private readonly PaymentContext _context;

		public PaymentRepository(PaymentContext context)
		{
			_context = context;
		}

		public void Create(Payment entity)
		{
			_context.Payments.Add(entity);
		}

		public async Task Delete(long id)
		{
			var entity = await GetById(id);
			_context.Payments.Remove(entity);
		}

		public async Task<IList<Payment>> GetAll()
		{
			return await _context.Payments.ToListAsync();
		}

		public async Task<Payment> GetById(long id)
		{
			return await _context.Payments.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Payment entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
