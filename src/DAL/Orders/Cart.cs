namespace DAL.Orders
{
	public sealed class Cart
	{
		public Guid Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
