using DAL.Abstraction;
using Entities.Payments;
using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Repository;

internal class PaymentRepository(OrderContext context) :
	GenericRepository<Payment, long, OrderContext>(context),
	IPaymentRepository
{
	public async Task<Payment> GetPaymentWithRelatedInfo(long id)
	{
		var payment = await Payments.Repository.PaymentRepository.IncludeRelatedInfo(_collection)
			.FirstOrDefaultAsync(p => p.Id == id);

		return payment;
	}
}