using API.Abstraction.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using OrderAPI.Controllers;
using OrderAPI.DTOs;
using OrderApplication.Commands;
using OrderApplication.Entities;
using OrderApplication.Queries;
using TestsCore;

namespace OrderTests.API
{
	public class APITests
	{
		[Fact]
		public async Task GetDetails_ResultNull_NotFound()
		{
			var mediator = new Mock<IMediator>();
			mediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((CartDetails)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetDetails(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task GetDetails_CartItemsReturned_OkObjectResultWithCorrectResult()
		{
			var mediator = new Mock<IMediator>();
			var cart = new CartDetails
			{
				Items =
				[
					new () { EventSeat = new()},
					new () { EventSeat = new()},
				]
			};
			mediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(cart);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.GetDetails(Guid.NewGuid());
			var value = (result as OkObjectResult)?.Value as Resource<CartDetails>;

			Assert.NotNull(value?.Value?.Items);
			Assert.Equal(cart.Items.Count, value.Value.Items.Count);
		}

		[Fact]
		public async Task AddToCart_ResultNull_NotFoundWithMessage()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			mediator.Setup(m => m.Send(It.IsAny<AddItemToCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Result<Unit>)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as NotFoundObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task AddToCart_ResultSuccess_OkWithCartDetails()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			var successResult = new Result<Unit>
			{
				Error = false,
				Value = Unit.Value
			};
			var cart = new CartDetails
			{
				Items =
				[
					new () { EventSeat = new()},
				]
			};
			mediator.Setup(m => m.Send(It.IsAny<AddItemToCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(successResult);
			mediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(cart);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as OkObjectResult)?.Value as Resource<CartDetails>;

			Assert.NotNull(value?.Value?.Items);
			Assert.Equal(cart.Items.Count, value.Value.Items.Count);
		}

		[Fact]
		public async Task AddToCart_ResultSuccess_ResultNull_ThrowsNullReference()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			var successResult = new Result<Unit>
			{
				Error = false,
				Value = Unit.Value
			};
			mediator.Setup(m => m.Send(It.IsAny<AddItemToCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(successResult);
			mediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((CartDetails)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);
			var cartItem = new CartItemInputModel();

			await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.AddToCart(cartItem, guid));
		}

		[Fact]
		public async Task AddToCart_ResultError_BadRequestWithMessage()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			var errorResult = new Result<Unit>
			{
				Error = true,
				ErrorMessage = "Message",
				Value = Unit.Value
			};
			mediator.Setup(m => m.Send(It.IsAny<AddItemToCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(errorResult);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as BadRequestObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task BookCartItems_ResultNull_NotFoundWithMessageContainingCartGuid()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			mediator.Setup(m => m.Send(It.IsAny<BookCartItemsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Result<long?>)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.BookCartItems(guid);
			var value = (result as NotFoundObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.Contains(guid.ToString(), value);
		}


		[Fact]
		public async Task BookCartItems_ResultError_BadRequestWithMessage()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			var errorResult = new Result<long?>
			{
				Error = true,
				ErrorMessage = "Message",
			};
			mediator.Setup(m => m.Send(It.IsAny<BookCartItemsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(errorResult);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.BookCartItems(guid);
			var value = (result as BadRequestObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task DeleteFromCart_ResultError_OkWithMessage()
		{
			await DeleteFromCartWithResultTest(false);
		}

		[Fact]
		public async Task DeleteFromCart_ResultSuccess_OkWithMessage()
		{
			await DeleteFromCartWithResultTest(true);
		}

		[Fact]
		public async Task DeleteFromCart_ResultNull_ThrowsNullReference()
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			mediator.Setup(m => m.Send(It.IsAny<DeleteItemFromCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Result<Unit>)null);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			await Assert.ThrowsAsync<NullReferenceException>(async () =>
				await controller.DeleteFromCart(guid, default, default));
		}

		private static async Task DeleteFromCartWithResultTest(bool error)
		{
			var mediator = new Mock<IMediator>();
			var guid = Guid.NewGuid();
			var errorResult = new Result<Unit>
			{
				Error = error,
				ErrorMessage = "Message",
				Value = Unit.Value
			};
			mediator.Setup(m => m.Send(It.IsAny<DeleteItemFromCartCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(errorResult);
			var linkGenerator = LinkGeneratorMock.GetLinkGeneratorMock();
			var controller = GetController(mediator.Object, linkGenerator.Object);

			var result = await controller.DeleteFromCart(guid, default, default);
			var value = (result as OkObjectResult).Value as Resource<string>;

			Assert.NotNull(value?.Value);
			Assert.NotEmpty(value?.Value);
		}

		private static CartsController GetController(IMediator mediator, LinkGenerator linkGenerator)
		{
			var controller = new CartsController(mediator, linkGenerator);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}