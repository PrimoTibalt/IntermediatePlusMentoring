using API.Abstraction.Helpers;
using OrderAPI.DTOs;
using Refit;

namespace OrderTests
{
	public interface ICartApi
	{
		[Put("/orders/carts/{guid}/book-{**method}")]
		Task<HttpResponseMessage> BookPessimistic(Guid guid, string method);

		[Post("/orders/carts/{guid}")]
		Task AddItemToCart(CartItemInputModel model, Guid guid);
	}
}
