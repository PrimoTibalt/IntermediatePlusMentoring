using DAL.Events;
using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repository
{
	internal class EventSeatRepository : GenericRepository<EventSeat, long, OrderContext>, IEventSeatRepository
	{
		public EventSeatRepository(OrderContext context) : base(context) { }

		public async Task<EventSeat> GetBy(int eventId, long seatId)
		{
			return await _collection.FirstOrDefaultAsync(es => es.EventId == eventId && es.Id == seatId);
		}

		public async Task<IList<EventSeat>> GetByIds(params long[] ids)
		{
			return await _collection.Where(es => ids.Contains(es.Id)).ToListAsync();
		}

		public IQueryable<EventSeat> GetByIdsQueryable(params long[] ids)
		{
			return _collection.Where(es => ids.Contains(es.Id));
		}
	}
}