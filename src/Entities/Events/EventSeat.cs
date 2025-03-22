using Entities.Venues;

namespace Entities.Events
{
	public sealed class EventSeat
	{
		public long Id { get; set; }
		public int EventId { get; set; }
		public int SeatId { get; set; }
		public int PriceId { get; set; }
		public Event Event { get; set; }
		public Seat Seat { get; set; }
		public Price Price { get; set; }
		public int Status { get; set; }
	}
}
