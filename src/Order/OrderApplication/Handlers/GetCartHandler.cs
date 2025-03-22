using AutoMapper;
using MediatR;
using OrderApplication.Entities;
using OrderApplication.Queries;
using OrderApplication.Repository;

namespace OrderApplication.Handlers
{
	public class GetCartHandler : IRequestHandler<GetCartQuery, CartDetails>
	{
		private readonly ICartRepository _repository;
		private readonly IMapper _mapper;

		public GetCartHandler(ICartRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<CartDetails> Handle(GetCartQuery request, CancellationToken cancellationToken)
		{
			var items = await _repository.GetItemsFull(request.Id);
			var result = new CartDetails
			{
				Items = _mapper.Map<IList<CartItemDetails>>(items)
			};
			return result;
		}
	}
}