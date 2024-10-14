using AutoMapper;
using DAL.Orders;
using DAL.Orders.Repository;
using MediatR;
using OrderApplication.Entities;
using OrderApplication.Queries;

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
                Items = _mapper.Map<IList<CartItem>>(items)
            };
            return result;
        }
    }
}