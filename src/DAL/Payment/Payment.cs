using DAL.Order;

namespace DAL.Payment
{
	public sealed class Payment
	{
		public int Id { get; set; }
		public Guid CartId { get; set; }
		public Cart Cart { get; set; }
		public string Status { get; set; }
	}
}
