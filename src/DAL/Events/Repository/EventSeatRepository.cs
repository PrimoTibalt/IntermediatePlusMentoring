using Microsoft.EntityFrameworkCore;

namespace DAL.Events.Repository
{
    internal class EventSeatRepository : GenericRepository<EventSeat, long, EventContext>, IEventSeatRepository
    {
        public EventSeatRepository(EventContext context) : base(context) {}

        public async Task<IList<EventSeat>> GetEventSectionSeats(int eventId, int sectionId)
        {
            return await _collection.Include(es => es.Price)
                .Include(es => es.Seat)
                .ThenInclude(es => es.Row)
                .Where(es => es.EventId == eventId && es.Seat.Row.SectionId == sectionId)
                .ToListAsync();
        }
    }
}