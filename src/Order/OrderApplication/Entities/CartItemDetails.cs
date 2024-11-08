namespace OrderApplication.Entities
{
	public class CartItemDetails
	{
		public Guid CartId { get; set; }
		public EventSeatDetails EventSeat { get; set; }
		public decimal Price { get; set; }
	}
}
