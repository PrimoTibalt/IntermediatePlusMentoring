using DAL.Events;
using EventApplication.Core;

namespace EventApplication.Entities
{
    public class SeatDetails
    {
        public long Id { get; set; }        
        public int RowId { get; set; }
        public int SectionId { get; set; }
        public int VenueSeatId { get; set; }
        public SeatStatus Status { get; set; }
        public Price Price { get; set; }
    }
}