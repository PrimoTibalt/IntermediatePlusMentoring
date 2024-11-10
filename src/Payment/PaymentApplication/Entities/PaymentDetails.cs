using DAL.Payments;

namespace PaymentApplication.Entities
{
	public class PaymentDetails
	{
		public long Id { get; set; }
		public Guid CartId { get; set; }
		public PaymentStatus Status { get; set; }
	}
}
