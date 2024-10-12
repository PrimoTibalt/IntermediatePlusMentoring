using DAL.Venues;
using DAL.Venues.Repository;
using MediatR;

namespace VenueApplication.Sections
{
	public class List
	{
		public class Query : IRequest<IList<Section>>
		{
			public int VenueId { get; set; }
		}

		public class RequestHandler : IRequestHandler<Query, IList<Section>>
		{
			private readonly ISectionRepository _sectionRepository;

			public RequestHandler(ISectionRepository sectionRepository)
			{
				_sectionRepository = sectionRepository;
			}

			public async Task<IList<Section>> Handle(Query request, CancellationToken cancellationToken)
			{
				var result = await _sectionRepository.GetByVenueId(request.VenueId, cancellationToken);
				return result;
			}
		}
	}
}
