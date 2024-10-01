using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repositories
{
	sealed class ManifestRepository : IGenericRepository<Manifest>
	{
		private readonly EventContext _context;

		public ManifestRepository(EventContext context)
		{
			_context = context;
		}

		public void Create(Manifest entity)
		{
			_context.Manifests.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			_context.Manifests.Remove(entity);
		}

		public async Task<IList<Manifest>> GetAll()
		{
			return await _context.Manifests.ToListAsync();
		}

		public async Task<Manifest> GetById(int id)
		{
			return await _context.Manifests.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Manifest entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
