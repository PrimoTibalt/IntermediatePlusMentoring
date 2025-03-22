namespace Entities.Payments
{
	public class PaymentSummary
	{
		public long Id { get; set; }
		public decimal Amount { get; set; }
		public string[] Events { get; set; }
		public int Status { get; set; }
		public string UserEmail { get; set; }
	}
}
