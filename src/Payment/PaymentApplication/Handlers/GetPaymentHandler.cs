using AutoMapper;
using DAL.Payments.Repository;
using MediatR;
using PaymentApplication.Entities;
using PaymentApplication.Queries;

namespace PaymentApplication.Handlers
{
	public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentDetails>
	{
		private readonly IDapperPaymentRepository _repository;
		private readonly IMapper _mapper;

		public GetPaymentHandler(IDapperPaymentRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<PaymentDetails> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
		{
			var payment = await _repository.GetById(request.Id);
			return _mapper.Map<PaymentDetails>(payment);
		}
	}
}