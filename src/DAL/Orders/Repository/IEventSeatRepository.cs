using DAL.Events;

namespace DAL.Orders.Repository
{
	public interface IEventSeatRepository : IGenericRepository<EventSeat, long>
	{
		Task<EventSeat> GetBy(int eventId, long seatId);
	}
}