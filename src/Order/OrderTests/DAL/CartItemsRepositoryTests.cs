using DAL;
using DAL.Orders;
using DAL.Orders.Repository;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Providers;

namespace OrderTests.DAL
{
	public class CartItemsRepositoryTests
	{
		private static IServiceProvider serviceProvider =>
			ServiceConfigurationProvider.Get<OrderContext>(services => services.AddOrderRepositories());

		[Theory]
		[InlineData(1, 3553)]
		[InlineData(2, 12)]
		[InlineData(100, 56466468)]
		[InlineData(int.MaxValue, 50)]
		[InlineData(6443, long.MaxValue)]
		public async Task Delete_ExistingValues_ReturnNullOnGetDeleted(int eventId, long seatId)
		{
			var cartId = Guid.NewGuid();
			var repository = serviceProvider.GetService<ICartItemRepository>();
			var cartItem = new CartItem
			{
				CartId = cartId,
				EventSeatId = seatId,
				EventSeat = new() { Id = seatId, EventId = eventId, PriceId = 1 },
				PriceId = 1,
			};
			await repository.Create(cartItem);
			await repository.Save();

			await repository.Delete(cartItem.Id);
			await repository.Save();
			var result = await repository.GetById(cartItem.Id);

			Assert.Null(result);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(2, 2)]
		[InlineData(100, 56466466)]
		[InlineData(int.MaxValue, 500)]
		[InlineData(64666646, long.MaxValue)]
		[InlineData(int.MinValue, 743533)]
		[InlineData(664622222, long.MinValue)]
		public async Task GetBy_ExistingValues_AddedRetrieved(int eventId, long seatId)
		{
			var cartId = Guid.NewGuid();
			var repository = serviceProvider.GetService<ICartItemRepository>();
			var cartItem = new CartItem
			{
				CartId = cartId,
				EventSeatId = seatId,
				EventSeat = new() { Id = seatId, EventId = eventId, PriceId = 1 },
				PriceId = 1,
			};
			await repository.Create(cartItem);
			await repository.Save();

			var result = await repository.GetBy(cartId, eventId, seatId);

			Assert.NotNull(result);
			Assert.Equal(cartId, result.CartId);
			Assert.Equal(eventId, result.EventSeat.EventId);
			Assert.Equal(seatId, result.EventSeatId);
		}

		[Fact]
		public async Task GetBy_ExistingValues_NullOnNoExistingItems()
		{
			var repository = serviceProvider.GetService<ICartItemRepository>();
			var cartId1 = Guid.NewGuid();
			var cartId2 = Guid.NewGuid();
			int eventId1 = 1, seatId1 = 1;
			int eventId2 = eventId1+1, seatId2 = seatId1+1;
			var cartItem1 = new CartItem
			{
				CartId = cartId1,
				EventSeatId = seatId1,
				EventSeat = new() { Id = seatId1, EventId = eventId1, PriceId = 1 },
				PriceId = 1,
			};
			var cartItem2 = new CartItem
			{
				CartId = cartId2,
				EventSeatId = seatId2,
				EventSeat = new() { Id = seatId2, EventId = eventId2, PriceId = 1 },
				PriceId = 1,
			};
			await repository.Create(cartItem1);
			await repository.Create(cartItem2);
			await repository.Save();

			var result1 = await repository.GetBy(cartId1, eventId1, seatId2);
			var result2 = await repository.GetBy(cartId2, eventId2, seatId1);
			var result3 = await repository.GetBy(cartId2, eventId1, seatId2);
			var result4 = await repository.GetBy(cartId1, eventId2, seatId1);

			Assert.Null(result1);
			Assert.Null(result2);
			Assert.Null(result3);
			Assert.Null(result4);
		}
	}
}
