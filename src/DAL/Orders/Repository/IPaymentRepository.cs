using DAL.Payments;

namespace DAL.Orders.Repository;

public interface IPaymentRepository : IGenericRepository<Payment, long>
{
	Task<Payment> GetPaymentWithRelatedInfo(long id);
}