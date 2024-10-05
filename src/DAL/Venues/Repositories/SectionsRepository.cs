using Microsoft.EntityFrameworkCore;

namespace DAL.Venues.Repositories
{
	sealed class SectionsRepository : IGenericRepository<Section, int>
	{
		private readonly VenueContext _context;

		public SectionsRepository(VenueContext context)
		{
			_context = context;
		}

		public async Task<IList<Section>> GetAll()
		{
			return await _context.Sections.ToListAsync();
		}

		public void Create(Section entity)
		{
			_context.Sections.Add(entity);
		}

		public async Task Delete(int id)
		{
			var section = await GetById(id);

			_context.Sections.Remove(section);
		}

		public async Task<Section> GetById(int id)
		{
			return await _context.Sections.Include(s => s.Rows).FirstOrDefaultAsync(s => s.Id == id);
		}

		public void Update(Section entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
