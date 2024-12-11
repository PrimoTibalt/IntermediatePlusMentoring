using Microsoft.AspNetCore.Http.HttpResults;
using PaymentAPI.Endpoints;
using PaymentApplication.Commands;
using PaymentApplication.Entities;
using PaymentApplication.Queries;
using TestsCore;

namespace PaymentTests
{
	public class APITests
	{
		[Fact]
		public async Task GetById_NullResult_NotFound()
		{
			var mediator = MediatorMockObjectBuilder.Get<GetPaymentQuery, PaymentDetails>(null);

			var result = await PaymentEndpoints.GetPaymentById(123, mediator);

			Assert.NotNull(result);
			Assert.IsType<NotFound<string>>(result);
		}

		[Fact]
		public async Task GetById_WithResult_OkWithPayment()
		{
			var paymentId = 123;
			var mediator = MediatorMockObjectBuilder.Get<GetPaymentQuery, PaymentDetails>(new PaymentDetails { Id = paymentId });

			var result = await PaymentEndpoints.GetPaymentById(666, mediator);
			var payment = (result as Ok<PaymentDetails>)?.Value;

			Assert.NotNull(payment);
			Assert.Equal(paymentId, payment.Id);
		}

		[Fact]
		public async Task Fail_SuccessResult_Ok()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, ProcessPaymentResult>(new() { Success = true });

			var result = await PaymentEndpoints.FailPayment(123, mediator);

			Assert.NotNull(result);
			Assert.IsType<Ok>(result);
		}

		[Fact]
		public async Task Fail_FailedResult_BadRequestWithMessage()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, ProcessPaymentResult>(new() { Success = false });

			var result =  await PaymentEndpoints.FailPayment(123, mediator);
			var message = (result as BadRequest<string>).Value;

			Assert.NotNull(message);
			Assert.NotEmpty(message);
		}

		[Fact]
		public async Task Complete_SuccessResult_Ok()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, ProcessPaymentResult>(new() { Success = true });

			var result = await PaymentEndpoints.CompletePayment(123, mediator);

			Assert.NotNull(result);
			Assert.IsType<Ok>(result);
		}

		[Fact]
		public async Task Complete_FailedResult_BadRequestWithMessage()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, ProcessPaymentResult>(new() { Success = false });

			var result = await PaymentEndpoints.CompletePayment(123, mediator);
			var message = (result as BadRequest<string>).Value;

			Assert.NotNull(message);
			Assert.NotEmpty(message);
		}
	}
}