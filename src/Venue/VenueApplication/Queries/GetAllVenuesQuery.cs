using Entities.Venues;
using MediatR;

namespace VenueApplication.Queries
{
    public class GetAllVenuesQuery : IRequest<IList<Venue>> { }
}