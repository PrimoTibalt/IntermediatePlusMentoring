namespace DAL.Payments.Repository
{
	public interface IPaymentRepository : IGenericRepository<Payment, long>
	{
		Task<Payment> GetPaymentWithRelatedInfo(long id);
		Task<bool> CompletePayment(long id);
		Task<bool> FailPayment(long id);
	}
}