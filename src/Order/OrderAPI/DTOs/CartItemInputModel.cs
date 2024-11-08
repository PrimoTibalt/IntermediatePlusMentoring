using System.ComponentModel.DataAnnotations;

namespace OrderAPI.DTOs
{
	public class CartItemInputModel
	{
		[Required]
		public int EventId { get; set; }
		[Required]
		public long SeatId { get; set; }
		[Required]
		public int UserId { get; set; }
	}
}