using DAL.Payments;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Controllers;
using PaymentApplication.Commands;
using PaymentApplication.Entities;
using PaymentApplication.Queries;
using TestsCore;
using TestsCore.Providers;

namespace PaymentTests
{
	public class APITests
	{
		[Fact]
		public async Task GetById_NullResult_NotFound()
		{
			var mediator = MediatorMockObjectBuilder.Get<GetPaymentQuery, PaymentDetails>(null);
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result = await controller.GetById(123);

			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
		}

		[Fact]
		public async Task GetById_WithResult_OkWithPayment()
		{
			var paymentId = 123;
			var mediator = MediatorMockObjectBuilder.Get<GetPaymentQuery, PaymentDetails>(new PaymentDetails { Id = paymentId });
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result = await controller.GetById(666);
			var payment = (result as OkObjectResult)?.Value as Payment;

			Assert.NotNull(payment);
			Assert.Equal(paymentId, payment.Id);
		}

		[Fact]
		public async Task Fail_SuccessResult_Ok()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, bool>(true);
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result = await controller.Fail(123);

			Assert.NotNull(result);
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task Fail_FailedResult_BadRequestWithMessage()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, bool>(false);
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result =  await controller.Fail(123);
			var message = (result as BadRequestObjectResult).Value as string;

			Assert.NotNull(message);
			Assert.NotEmpty(message);
		}

		[Fact]
		public async Task Complete_SuccessResult_Ok()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, bool>(true);
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result = await controller.Complete(123);

			Assert.NotNull(result);
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task Complete_FailedResult_BadRequestWithMessage()
		{
			var mediator = MediatorMockObjectBuilder.Get<ProcessPaymentCommand, bool>(false);
			var controller = ControllerProvider.Get<PaymentController>(mediator, false);

			var result = await controller.Complete(123);
			var message = (result as BadRequestObjectResult).Value as string;

			Assert.NotNull(message);
			Assert.NotEmpty(message);
		}
	}
}