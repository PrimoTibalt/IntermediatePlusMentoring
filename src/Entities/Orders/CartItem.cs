using Entities.Events;

namespace Entities.Orders
{
	public sealed class CartItem
	{
		public long Id { get; set; }
		public Guid CartId { get; set; }
		public Cart Cart { get; set; }
		public long EventSeatId { get; set; }
		public EventSeat EventSeat { get; set; }
		public int PriceId { get; set; }
		public Price Price { get; set; }
	}
}
