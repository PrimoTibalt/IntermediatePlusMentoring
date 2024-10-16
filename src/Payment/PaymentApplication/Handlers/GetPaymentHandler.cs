using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using PaymentApplication.Queries;

namespace PaymentApplication.Handlers
{
    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, Payment>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Payment> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetById(request.Id);
        }
    }
}