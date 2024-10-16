using API.Abstraction.Helpers;
using MediatR;

namespace OrderApplication.Commands
{
    public class DeleteItemFromCartCommand : IRequest<Result<Unit>>
    {
        public Guid CartId { get; set; }
        public long SeatId { get; set; }
        public int EventId { get; set; }
    }
}