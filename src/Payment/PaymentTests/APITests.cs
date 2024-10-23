using DAL.Payments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentAPI.Controllers;
using PaymentApplication.Commands;
using PaymentApplication.Queries;
using TestsCore;

namespace PaymentTests
{
	public class APITests
	{
		[Fact]
		public async Task GetById_NullResult_NotFound()
		{
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Payment)null);
			var controller = GetController(mediator.Object);

			var result = await controller.GetById(123);

			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
		}

		[Fact]
		public async Task GetById_WithResult_OkWithPayment()
		{
			var mediator = new Mock<IMediator>();
			var paymentId = 123;
			mediator.Setup(m => m.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new Payment { Id = paymentId });
			var controller = GetController(mediator.Object);

			var result = await controller.GetById(666);
			var payment = (result as OkObjectResult)?.Value as Payment;

			Assert.NotNull(payment);
			Assert.Equal(paymentId, payment.Id);
		}

		[Fact]
		public async Task Fail_SuccessResult_Ok()
		{
			await ProcessPaymentCommandTestOk(false);
		}

		[Fact]
		public async Task Fail_FailedResult_BadRequestWithMessage()
		{
			await ProcessPaymentCommandTestBadRequest(false);
		}

		[Fact]
		public async Task Complete_SuccessResult_Ok()
		{
			await ProcessPaymentCommandTestOk(true);
		}

		[Fact]
		public async Task Complete_FailedResult_BadRequestWithMessage()
		{
			await ProcessPaymentCommandTestBadRequest(true);
		}

		private static async Task ProcessPaymentCommandTestBadRequest(bool complete)
		{
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(false);
			var controller = GetController(mediator.Object);

			var result = complete ? await controller.Complete(123)
				: await controller.Fail(123);
			var message = (result as BadRequestObjectResult).Value as string;

			Assert.NotNull(message);
			Assert.NotEmpty(message);
		}

		private static async Task ProcessPaymentCommandTestOk(bool complete)
		{
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(true);
			var controller = GetController(mediator.Object);

			var result = complete ? await controller.Complete(123)
				: await controller.Fail(123);

			Assert.NotNull(result);
			Assert.IsType<OkResult>(result);
		}

		private static PaymentController GetController(IMediator mediator)
		{
			var controller = new PaymentController(mediator);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}