using Entities.Events;

namespace OrderApplication.Entities
{
	public class EventSeatDetails
	{
		public long Id { get; set; }
		public int EventId { get; set; }
		public int SeatId { get; set; }
		public SeatStatus Status { get; set; }
	}
}
