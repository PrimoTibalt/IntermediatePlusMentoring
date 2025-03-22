using DAL;
using DAL.Orders;
using Entities.Orders;
using Microsoft.Extensions.DependencyInjection;
using OrderApplication.Repository;
using TestsCore.Providers;

namespace OrderTests.DAL
{
	public class CartRepositoryTests
	{
		private static IServiceProvider serviceProvider =>
			ServiceConfigurationProvider.Get<OrderContext>(services => services.AddOrderRepositories());

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(35)]
		[InlineData(13425)]
		[InlineData(int.MaxValue)]
		[InlineData(9999999)]
		public async Task Create_Empty_AddedRetrieved(int userId)
		{
			var repository = serviceProvider.GetService<ICartRepository>();
			var cartId = Guid.NewGuid();
			var cart = new Cart
			{
				Id = cartId,
				UserId = userId
			};
			await repository.Create(cart);
			var result = await repository.GetById(cartId);

			Assert.NotNull(result);
			Assert.Equal(cartId, result.Id);
			Assert.Equal(userId, result.UserId);
		}

		[Fact]
		public async Task GetItemsFull_ExistingValues_ReturnNullOnGetWithIncorrectId()
		{
			var repository = serviceProvider.GetService<ICartRepository>();
			var cartCount = 100;
			for (int i = 0; i < cartCount; i++)
			{
				await repository.Create(new Cart
				{
					Id = Guid.NewGuid(),
					UserId = i,
					CartItems = [
								new() { Id = cartCount + i },
												new() { Id = 2*cartCount + i }
						],
				});
				await repository.Save();
			}

			var result = await repository.GetItemsFull(Guid.NewGuid());

			Assert.Null(result);
		}

		[Fact]
		public async Task GetItemsWithEventSeat_ExistingValues_ReturnsWithEventSeat()
		{
			var repository = serviceProvider.GetService<ICartRepository>();
			var cartCount = 100;
			var ids = await CreateCartsWithSeatsAndPrices(cartCount, repository);
			var results = new IList<CartItem>[cartCount];

			foreach (var idIndexPair in ids.Select((Id, Index) => new { Id, Index }))
			{
				results[idIndexPair.Index] = await repository.GetItemsWithEventSeat(idIndexPair.Id);
			}

			Assert.All(results, cartItems =>
			{
				Assert.NotNull(cartItems);
				Assert.NotEmpty(cartItems);
				Assert.All(cartItems, cartItem =>
				{
					Assert.NotNull(cartItem?.EventSeat);
				});
			});
		}

		[Fact]
		public async Task GetItemsFull_ExistingValues_ReturnsFull()
		{
			var repository = serviceProvider.GetService<ICartRepository>();
			var cartCount = 100;
			var ids = await CreateCartsWithSeatsAndPrices(cartCount, repository);
			var results = new IList<CartItem>[cartCount];

			foreach (var idIndexPair in ids.Select((Id, Index) => new { Id, Index }))
			{
				results[idIndexPair.Index] = await repository.GetItemsFull(idIndexPair.Id);
			}

			Assert.All(results, cartItems =>
			{
				Assert.NotNull(cartItems);
				Assert.NotEmpty(cartItems);
				Assert.All(cartItems, cartItem =>
				{
					Assert.NotNull(cartItem?.EventSeat);
					Assert.NotNull(cartItem?.Price);
				});
			});
		}

		private static async Task<Guid[]> CreateCartsWithSeatsAndPrices(int cartCount, ICartRepository repository)
		{
			var ids = new Guid[cartCount];
			for (int i = 0; i < cartCount; i++)
			{
				var cartId = Guid.NewGuid();
				await repository.Create(new Cart
				{
					Id = cartId,
					UserId = i,
					CartItems = [
						new() {
							Id = cartCount + i,
							EventSeat = new() { Id = cartCount + i },
							Price = new() { Id = cartCount + i },
							EventSeatId = cartCount + i,
							PriceId = cartCount + i
					},
					new() {
							Id = 2*cartCount + i,
							EventSeat = new() { Id = 4*cartCount + i },
							Price = new() { Id = 2*cartCount + i },
							EventSeatId = 4*cartCount + i,
							PriceId = 2*cartCount + i
					}],
				});
				ids[i] = cartId;
				await repository.Save();
			}

			return ids;
		}
	}
}
