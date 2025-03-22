using Entities.Payments;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentApplication.Commands;
using PaymentApplication.Repository;
using RegisterServicesSourceGenerator;

namespace PaymentApplication.Handlers
{
	[RegisterService<IRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>>(LifeTime.Transient)]
	public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>
	{
		private readonly IDapperPaymentRepository _paymentRepository;
		private readonly ILogger _logger;

		public ProcessPaymentHandler(
			IDapperPaymentRepository paymentRepository,
			ILogger<ProcessPaymentHandler> logger)
		{
			_paymentRepository = paymentRepository;
			_logger = logger;
		}

		public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
		{
			var payment = await _paymentRepository.GetById(request.Id);
			if (payment is null || payment.Status != (int)PaymentStatus.InProgress)
				return null;

			Func<long, Task<bool>> task = request.Complete ?
				_paymentRepository.CompletePayment :
				_paymentRepository.FailPayment;

			bool result;
			try
			{
				result = await task(request.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Operation on payment with id '{Id}' failed.", request.Id);
				result = false;
			}

			return new() { Success = result };
		}
	}
}