using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repositories
{
	sealed class EventRepository : IGenericRepository<Event, int>
	{
		private readonly EventContext _context;

		public EventRepository(EventContext context)
		{
			_context = context;
		}

		public void Create(Event entity)
		{
			_context.Events.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);

			_context.Events.Remove(entity);
		}

		public async Task<IList<Event>> GetAll()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<Event> GetById(int id)
		{
			return await _context.Events.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(Event entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
