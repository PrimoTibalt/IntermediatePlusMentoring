namespace Entities.Orders
{
	public sealed class Cart
	{
		public Guid Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public ICollection<CartItem> CartItems { get; set; }
	}
}
