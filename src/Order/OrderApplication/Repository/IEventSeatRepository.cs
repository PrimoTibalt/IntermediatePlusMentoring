using DAL.Abstraction;
using Entities.Events;

namespace OrderApplication.Repository
{
	public interface IEventSeatRepository : IGenericRepository<EventSeat, long>
	{
		Task<EventSeat> GetBy(int eventId, long seatId);
	}
}