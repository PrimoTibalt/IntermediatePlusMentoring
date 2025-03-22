using API.Abstraction.Helpers;
using Entities.Payments;
using MediatR;
using OrderApplication.Commands;
using OrderApplication.Repository;

namespace OrderApplication.Handlers
{
	public class BookCartItemsHandler(ICartRepository cartRepository,
		IPaymentRepository paymentRepository,
		IBookCartOperation bookCartOperation)
		: IRequestHandler<BookCartItemsCommand, Result<long?>>
	{
		private readonly ICartRepository _cartRepository = cartRepository;
		private readonly IPaymentRepository _paymentRepository = paymentRepository;
		private readonly IBookCartOperation _bookCartOperation = bookCartOperation;

		public async Task<Result<long?>> Handle(BookCartItemsCommand request, CancellationToken cancellationToken)
		{
			await _cartRepository.BeginTransaction();

			var result = await _bookCartOperation.TryBookCart(request.Id, request.OptimisticExecution);
			if (!result) return FailedUnavailableSeats(request.Id);
			var payment = await CreatePayment(request.Id);

			await _paymentRepository.Save();
			await _cartRepository.CommitTransaction();

			return Result<long?>.Success(payment.Id);
		}

		private async Task<Payment> CreatePayment(Guid id)
		{
			var payment = new Payment
			{
				CartId = id,
				Status = (int)PaymentStatus.InProgress,
			};
			await _paymentRepository.Create(payment);
			return payment;
		}

		private static Result<long?> FailedUnavailableSeats(Guid id) 
			=> Result<long?>.Failure($"Cart with id '{id}' contains unavailable seats.");
	}
}