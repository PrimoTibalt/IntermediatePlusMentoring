using AutoMapper;
using DAL.Orders;
using DAL.Orders.Repository;
using MediatR;
using OrderApplication.Entities;

namespace OrderApplication.Carts
{
    public class Details 
    {
        public class Query : IRequest<CartDetails>        
        {
            public Guid Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, CartDetails>
        {
            private readonly ICartRepository _repository;
            private readonly IMapper _mapper;

            public RequestHandler(ICartRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<CartDetails> Handle(Query request, CancellationToken cancellationToken)
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
}