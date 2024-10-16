using EventApplication.Entities;
using MediatR;

namespace EventApplication.Queries
{
    public class GetEventSectionSeatsQuery : IRequest<IList<SeatDetails>> 
    {
        public int EventId { get; set; }
        public int SectionId { get; set; }
    }
}