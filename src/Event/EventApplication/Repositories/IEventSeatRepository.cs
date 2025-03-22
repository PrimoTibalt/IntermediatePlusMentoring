using DAL.Abstraction;
using Entities.Events;

namespace EventApplication.Repositories
{
    public interface IEventSeatRepository : IGenericRepository<EventSeat, long>
    {
        Task<IList<EventSeat>> GetEventSectionSeats(int eventId, int sectionId);        
    }
}