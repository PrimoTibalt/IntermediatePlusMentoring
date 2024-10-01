using DAL.Venue;

namespace DAL.Events
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
		public string Status { get; set; }
	}
}
