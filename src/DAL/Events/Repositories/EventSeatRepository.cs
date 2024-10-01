using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repositories
{
	sealed class EventSeatRepository : IGenericRepository<EventSeat>
	{
		private readonly EventContext _context;

		public EventSeatRepository(EventContext context)
		{
			_context = context;
		}

		public void Create(EventSeat entity)
		{
			_context.EventSeats.Add(entity);
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			_context.EventSeats.Remove(entity);
		}

		public async Task<IList<EventSeat>> GetAll()
		{
			return await _context.EventSeats.ToListAsync();
		}

		public async Task<EventSeat> GetById(int id)
		{
			return await _context.EventSeats.FindAsync(id);
		}

		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}

		public void Update(EventSeat entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
