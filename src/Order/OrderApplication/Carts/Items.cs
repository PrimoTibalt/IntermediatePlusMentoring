using DAL.Orders;
using DAL.Orders.Repository;
using MediatR;

namespace OrderApplication.Carts
{
    public class Items
    {
        public class Query : IRequest<IList<CartItem>>        
        {
            public Guid Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, IList<CartItem>>
        {
            private readonly ICartRepository _repository;

            public RequestHandler(ICartRepository repository)
            {
                _repository = repository;
            }

            public async Task<IList<CartItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetItems(request.Id);
            }
        }
    }
}