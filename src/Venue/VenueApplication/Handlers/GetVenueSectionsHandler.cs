using DAL.Venues;
using DAL.Venues.Repository;
using MediatR;
using VenueApplication.Queries;

namespace VenueApplication.Handlers
{
    public class GetVenueSectionsHandler : IRequestHandler<GetVenueSectionsQuery, IList<Section>>
    {
        private readonly ISectionRepository _sectionRepository;

        public GetVenueSectionsHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<IList<Section>> Handle(GetVenueSectionsQuery request, CancellationToken cancellationToken)
        {
            return await _sectionRepository.GetByVenueId(request.VenueId, cancellationToken);
        }
    }

}