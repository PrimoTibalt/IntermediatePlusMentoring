using MediatR;

namespace PaymentApplication.Commands
{
    public class ProcessPaymentCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public bool Complete { get; set; }
    }
}