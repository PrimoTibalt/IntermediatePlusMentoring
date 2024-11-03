using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderAPI.DTOs;
using Refit;
using System.Net;
using Testcontainers.PostgreSql;

namespace OrderTests
{
	public class BookingConcurrencyTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;
		private readonly IContainer _cacheContainer;
		private readonly PostgreSqlContainer _dbContainer;

		public BookingConcurrencyTests(WebApplicationFactory<Program> factory)
		{
			_dbContainer = new PostgreSqlBuilder()
				.WithImage("postgres:17.0")
				.WithPortBinding("5404", "5432")
				.WithEnvironment("POSTGRES_PASSWORD", "postgres")
				.WithResourceMapping(new FileInfo("init.sql"), "/docker-entrypoint-initdb.d/")
				.WithCleanUp(true)
				.Build();
			_cacheContainer = new ContainerBuilder()
				.WithImage("redis:7.0")
				.WithPortBinding("6379", "6379")
				.WithCleanUp(true)
				.Build();
			_factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("QA"));
		}

		[Theory]
		[InlineData("pessimistic")]
		[InlineData("optimistic")]
		public async Task CartController_Book_OneSuccessfulBooking(string url)
		{
			await _dbContainer.StartAsync();
			await _cacheContainer.StartAsync();
			var apiClient = RestService.For<ICartApi>(_factory.CreateClient());
			List<Guid> guids = [];
			foreach (var _ in Enumerable.Range(0, 10))
			{
				var guid = Guid.NewGuid();
				var cartItemModels = Enumerable.Range(1, 4)
					.Select(index => new CartItemInputModel() { EventId = 1, SeatId = index, UserId = 1 })
					.ToArray();
				foreach (var cartItem in cartItemModels)
					await apiClient.AddItemToCart(cartItem, guid);
				guids.Add(guid);
			}

			List<HttpResponseMessage> responses = [];
			await Parallel.ForEachAsync(guids, async (guid, _) =>
			{
				responses.Add(await apiClient.BookPessimistic(guid, url));
			});

			Assert.Single(responses, result =>
			{
				return result.StatusCode == HttpStatusCode.OK;
			});
		}
	}
}
