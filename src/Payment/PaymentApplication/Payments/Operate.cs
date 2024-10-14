using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;

namespace PaymentApplication.Payments
{
    public class Operate
    {
        public class Command : IRequest<bool>
        {
            public long Id { get; set; }
            public bool Complete { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IPaymentRepository _paymentRepository;

            public RequestHandler(IPaymentRepository paymentRepository)
            {
                _paymentRepository = paymentRepository;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var payment = await _paymentRepository.GetById(request.Id);
                if (payment is null || payment.Status != PaymentStatus.InProgress.ToString().ToLowerInvariant())
                    return false;

                return request.Complete ? await _paymentRepository.CompletePayment(request.Id)
                                        : await _paymentRepository.FailPayment(request.Id);
            }
        }
    }
}