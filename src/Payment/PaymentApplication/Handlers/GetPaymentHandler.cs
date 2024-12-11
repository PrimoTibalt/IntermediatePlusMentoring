using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using PaymentApplication.Entities;
using PaymentApplication.Queries;
using RegisterServicesSourceGenerator;

namespace PaymentApplication.Handlers
{
	[RegisterService<IRequestHandler<GetPaymentQuery, PaymentDetails>>(LifeTime.Transient)]
	public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentDetails>
	{
		private readonly IDapperPaymentRepository _repository;

		public GetPaymentHandler(IDapperPaymentRepository repository)
		{
			_repository = repository;
		}

		public async Task<PaymentDetails> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
		{
			var payment = await _repository.GetById(request.Id);
			var paymentDetails = new PaymentDetails
			{
				Id = payment.Id,
				CartId = payment.CartId,
				Status = (PaymentStatus) payment.Status
			};
			return paymentDetails;
		}
	}
}