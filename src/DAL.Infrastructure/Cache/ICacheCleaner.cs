using DAL.Events;

namespace OrderApplication.Cache
{
	public interface ICacheCleaner
	{
		Task CleanEventSeatsCache(IList<EventSeat> seats);
	}
}
