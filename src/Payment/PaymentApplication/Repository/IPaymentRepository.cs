using DAL.Abstraction;
using Entities.Payments;

namespace PaymentApplication.Repository
{
	public interface IPaymentRepository : IGenericRepository<Payment, long>
	{
		Task<Payment> GetPaymentWithRelatedInfo(long id);
		Task<bool> CompletePayment(long id);
		Task<bool> FailPayment(long id);
	}
}