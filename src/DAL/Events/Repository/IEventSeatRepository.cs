namespace DAL.Events.Repository
{
    public interface IEventSeatRepository : IGenericRepository<EventSeat, long>
    {
        Task<IList<EventSeat>> GetEventSectionSeats(int eventId, int sectionId);        
    }
}