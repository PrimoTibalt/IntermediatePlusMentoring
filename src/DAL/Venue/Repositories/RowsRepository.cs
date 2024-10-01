using Microsoft.EntityFrameworkCore;

namespace DAL.Venue.Repositories
{
	sealed class RowsRepository : IGenericRepository<Row>
	{
		private readonly VenueContext _context;

		public RowsRepository(VenueContext context)
		{
			_context = context;
		}

		public void Create(Row entity)
		{
			_context.Rows.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			_context.Rows.Remove(entity);
		}

		public async Task<IList<Row>> GetAll()
		{
			return await _context.Rows.ToListAsync();
		}

		public async Task<Row> GetById(int id)
		{
			return await _context.Rows.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Row entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
