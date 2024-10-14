using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;

namespace PaymentApplication.Payments
{
    public class Details
    {
        public class Query : IRequest<Payment>
        {
            public long Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, Payment>
        {
            private readonly IPaymentRepository _repository;

            public RequestHandler(IPaymentRepository repository)
            {
                _repository = repository;
            }

            public async Task<Payment> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.Id);
            }
        }
    }
}