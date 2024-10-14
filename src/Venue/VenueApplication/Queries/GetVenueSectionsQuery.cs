using DAL.Venues;
using MediatR;

namespace VenueApplication.Queries
{
    public class GetVenueSectionsQuery : IRequest<IList<Section>>
	{
		public int VenueId { get; set; }
	}
}