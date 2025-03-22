using DAL.Abstraction;
using Entities.Payments;

namespace OrderApplication.Repository;

public interface IPaymentRepository : IGenericRepository<Payment, long>
{
	Task<Payment> GetPaymentWithRelatedInfo(long id);
}