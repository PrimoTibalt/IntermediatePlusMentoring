using API.Abstraction.Helpers;
using Entities.Events;
using MediatR;

namespace OrderApplication.Commands
{
    public class DeleteItemFromCartCommand : IRequest<Result<EventSeat>>
    {
        public Guid CartId { get; set; }
        public long SeatId { get; set; }
        public int EventId { get; set; }
    }
}