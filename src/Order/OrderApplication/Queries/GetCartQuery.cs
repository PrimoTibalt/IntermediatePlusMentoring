using MediatR;
using OrderApplication.Entities;

namespace OrderApplication.Queries
{
    public class GetCartQuery : IRequest<CartDetails>
    {
        public Guid Id { get; set; }
    }
}