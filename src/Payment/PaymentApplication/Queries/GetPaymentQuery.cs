using MediatR;
using PaymentApplication.Entities;

namespace PaymentApplication.Queries
{
	public class GetPaymentQuery : IRequest<PaymentDetails>
	{
		public long Id { get; set; }
	}
}