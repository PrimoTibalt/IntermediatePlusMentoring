using DAL.Venues;
using DAL.Venues.Repository;
using MediatR;
using VenueApplication.Queries;

namespace VenueApplication.Handlers
{
    public class GetVenueSectionsHandler : IRequestHandler<GetVenueSectionsQuery, IList<Section>>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IVenueRepository _venueRepository;

        public GetVenueSectionsHandler(ISectionRepository sectionRepository, IVenueRepository venueRepository)
        {
            _sectionRepository = sectionRepository;
            _venueRepository = venueRepository;
        }

        public async Task<IList<Section>> Handle(GetVenueSectionsQuery request, CancellationToken cancellationToken)
        {
            var venue = await _venueRepository.GetById(request.VenueId);
            if (venue == null)
              return null;

            return await _sectionRepository.GetByVenueId(request.VenueId);
        }
    }

}