using DAL.Abstraction;
using Entities.Payments;

namespace DAL.Orders.Repository;

public interface IPaymentRepository : IGenericRepository<Payment, long>
{
	Task<Payment> GetPaymentWithRelatedInfo(long id);
}