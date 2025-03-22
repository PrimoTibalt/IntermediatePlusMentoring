using DAL.Abstraction;
using Entities.Events;
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
	}
}