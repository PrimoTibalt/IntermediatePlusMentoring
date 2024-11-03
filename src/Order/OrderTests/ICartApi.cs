using OrderAPI.DTOs;
using Refit;

namespace OrderTests
{
	public interface ICartApi
	{
		[Put("/orders/carts/{guid}/book-{**method}")]
		Task<HttpResponseMessage> Book(Guid guid, string method);

		[Post("/orders/carts/{guid}")]
		Task AddItemToCart(CartItemInputModel model, Guid guid);
	}
}
