using DAL.Payments;
using DAL.Payments.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentApplication.Commands;

namespace PaymentApplication.Handlers
{
	public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, bool>
	{
		private readonly IPaymentRepository _paymentRepository;

		public ProcessPaymentHandler(IPaymentRepository paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = await _paymentRepository.GetById(request.Id);
			if (payment is null || payment.Status != (int)PaymentStatus.InProgress)
				return false;

			bool result;
			try
			{
				result = request.Complete ? await _paymentRepository.CompletePayment(request.Id)
																	: await _paymentRepository.FailPayment(request.Id);
			}
			catch (DbUpdateException)
			{
				result = false;
			}

			return result;
		}
	}
}