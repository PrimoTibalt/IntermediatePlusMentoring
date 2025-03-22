using MediatR;
using VenueApplication.Entities;

namespace VenueApplication.Queries
{
	public class GetVenueSectionsQuery : IRequest<IList<SectionDetails>>
	{
		public int VenueId { get; set; }
	}
}