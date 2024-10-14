using API.Abstraction.Helpers;
using MediatR;

namespace OrderApplication.Commands
{
    public class BookCartItemsCommand : IRequest<Result<long?>>
    {
        public Guid Id { get; set; }
    }
}