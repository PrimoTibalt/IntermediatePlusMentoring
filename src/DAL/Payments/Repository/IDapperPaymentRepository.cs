namespace DAL.Payments.Repository
{
	public interface IDapperPaymentRepository
	{
		Task<Payment> GetById(long id); 
		Task<PaymentSummary> GetPaymentSummary(long id);
		Task<bool> CompletePayment(long id);
		Task<bool> FailPayment(long id);
	}
}
