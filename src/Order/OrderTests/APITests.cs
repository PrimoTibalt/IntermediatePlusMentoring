using API.Abstraction.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Controllers;
using OrderAPI.DTOs;
using OrderApplication.Commands;
using OrderApplication.Entities;
using OrderApplication.Queries;
using TestsCore;
using TestsCore.Providers;

namespace OrderTests.API
{
	public class APITests
	{
		[Fact]
		public async Task GetDetails_ResultNull_NotFound()
		{
			var mediator = MediatorMockObjectBuilder.Get<GetCartQuery, CartDetails>(null);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.GetDetails(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task GetDetails_CartItemsReturned_OkObjectResultWithCorrectResult()
		{
			var cart = new CartDetails
			{
				Items =
				[
					new () { EventSeat = new()},
					new () { EventSeat = new()},
				]
			};
			var mediator = MediatorMockObjectBuilder.Get<GetCartQuery, CartDetails>(cart);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.GetDetails(Guid.NewGuid());
			var value = (result as OkObjectResult)?.Value as Resource<CartDetails>;

			Assert.NotNull(value?.Value?.Items);
			Assert.Equal(cart.Items.Count, value.Value.Items.Count);
		}

		[Fact]
		public async Task AddToCart_ResultNull_NotFoundWithMessage()
		{
			var guid = Guid.NewGuid();
			var mediator = MediatorMockObjectBuilder.Get<AddItemToCartCommand, Result<Unit>>(null);
			var controller = ControllerProvider.Get<CartsController>(mediator);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as NotFoundObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task AddToCart_ResultSuccess_OkWithCartDetails()
		{
			var guid = Guid.NewGuid();
			var successResult = new Result<Unit>
			{
				Error = false,
				Value = Unit.Value
			};
			var cart = new CartDetails
			{
				Items = [ new () { EventSeat = new()} ]
			};
			var mediator = new MediatorMockObjectBuilder()
				.AppendSetup<AddItemToCartCommand, Result<Unit>>(successResult)
				.AppendSetup<GetCartQuery, CartDetails>(cart)
				.GetObject();
			var controller = ControllerProvider.Get<CartsController>(mediator);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as OkObjectResult)?.Value as Resource<CartDetails>;

			Assert.NotNull(value?.Value?.Items);
			Assert.Equal(cart.Items.Count, value.Value.Items.Count);
		}

		[Fact]
		public async Task AddToCart_ResultError_BadRequestWithMessage()
		{
			var guid = Guid.NewGuid();
			var errorResult = new Result<Unit>
			{
				Error = true,
				ErrorMessage = "Message",
				Value = Unit.Value
			};
			var mediator = MediatorMockObjectBuilder.Get<AddItemToCartCommand, Result<Unit>>(errorResult);
			var controller = ControllerProvider.Get<CartsController>(mediator);
			var cartItem = new CartItemInputModel();

			var result = await controller.AddToCart(cartItem, guid);
			var value = (result as BadRequestObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task BookCartItems_ResultNull_NotFoundWithMessageContainingCartGuid()
		{
			var guid = Guid.NewGuid();
			var mediator = MediatorMockObjectBuilder.Get<BookCartItemsCommand, Result<long?>>(null);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.BookCartItems(guid);
			var value = (result as NotFoundObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.Contains(guid.ToString(), value);
		}

		[Fact]
		public async Task BookCartItems_ResultError_BadRequestWithMessage()
		{
			var guid = Guid.NewGuid();
			var errorResult = new Result<long?>
			{
				Error = true,
				ErrorMessage = "Message",
			};
			var mediator = MediatorMockObjectBuilder.Get<BookCartItemsCommand, Result<long?>>(errorResult);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.BookCartItems(guid);
			var value = (result as BadRequestObjectResult)?.Value as string;

			Assert.NotNull(value);
			Assert.NotEmpty(value);
		}

		[Fact]
		public async Task DeleteFromCart_ResultError_OkWithMessage()
		{
			var guid = Guid.NewGuid();
			var errorResult = new Result<Unit>
			{
				Error = false,
				ErrorMessage = "Message",
				Value = Unit.Value
			};
			var mediator = MediatorMockObjectBuilder.Get<DeleteItemFromCartCommand, Result<Unit>>(errorResult);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.DeleteFromCart(guid, default, default);
			var value = (result as OkObjectResult).Value as Resource<string>;

			Assert.NotNull(value?.Value);
			Assert.NotEmpty(value?.Value);

		}

		[Fact]
		public async Task DeleteFromCart_ResultSuccess_OkWithMessage()
		{
			var guid = Guid.NewGuid();
			var errorResult = new Result<Unit>
			{
				Error = true,
				ErrorMessage = "Message",
				Value = Unit.Value
			};
			var mediator = MediatorMockObjectBuilder.Get<DeleteItemFromCartCommand, Result<Unit>>(errorResult);
			var controller = ControllerProvider.Get<CartsController>(mediator);

			var result = await controller.DeleteFromCart(guid, default, default);
			var value = (result as OkObjectResult).Value as Resource<string>;

			Assert.NotNull(value?.Value);
			Assert.NotEmpty(value?.Value);
		}
	}
}