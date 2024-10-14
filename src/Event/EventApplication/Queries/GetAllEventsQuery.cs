using DAL.Events;
using MediatR;

namespace EventApplication.Queries
{
    public class GetAllEventsQuery : IRequest<IList<Event>> {}
}