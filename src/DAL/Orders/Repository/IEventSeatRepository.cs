using DAL.Events;

namespace DAL.Orders.Repository
{
	public interface IEventSeatRepository : IGenericRepository<EventSeat, long>
	{
		Task<EventSeat> GetBy(int eventId, long seatId);
		Task<IList<EventSeat>> GetByIds(params long[] ids);
		IQueryable<EventSeat> GetByIdsQueryable(params long[] ids);
	}
}