using MediatR;

namespace PaymentApplication.Commands
{
	public class ProcessPaymentCommand : IRequest<ProcessPaymentResult>
    {
		public long Id { get; set; }
		public bool Complete { get; set; }
	}

	/// Required because MediatR uses MakeGenericType() and it is AOT compatible only if arguments are reference types
	public class ProcessPaymentResult
	{
		public bool Success { get; set; }
	}
}