using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderAPI.DTOs;
using Refit;
using System.Collections.Concurrent;
using System.Net;

namespace OrderTests
{
	public class BookingConcurrencyTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		public BookingConcurrencyTests(WebApplicationFactory<Program> factory)
		{
			// to avoid conflicts with local docker
			_factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("QA"));
		}

		[Theory]
		[InlineData("pessimistic")]
		[InlineData("optimistic")]
		public async Task CartController_Book_OneSuccessfulBooking(string url)
		{
			await new EndToEndExecutioner().Execute(async () =>
			{
				var apiClient = RestService.For<ICartApi>(_factory.CreateClient());
				ConcurrentBag<Guid> guids = [];
				await Parallel.ForEachAsync(Enumerable.Range(0, 1000), async (_, _) =>
				{
					var guid = Guid.NewGuid();
					var cartItemModels = Enumerable.Range(1, 4)
						.Select(index => new CartItemInputModel() { EventId = 1, SeatId = index, UserId = 1 })
						.ToArray();
					foreach (var cartItem in cartItemModels)
						await apiClient.AddItemToCart(cartItem, guid);
					guids.Add(guid);
				});

				ConcurrentBag<HttpResponseMessage> responses = [];
				await Parallel.ForEachAsync(guids, async (guid, _) =>
				{
					responses.Add(await apiClient.Book(guid, url));
				});

				Assert.Single(responses, result =>
				{
					return result.StatusCode == HttpStatusCode.OK;
				});
			});
		}
	}
}
