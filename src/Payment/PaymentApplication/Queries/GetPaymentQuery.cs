using DAL.Payments;
using MediatR;

namespace PaymentApplication.Queries
{
    public class GetPaymentQuery : IRequest<Payment>
    {
        public long Id { get; set; }
    }
}