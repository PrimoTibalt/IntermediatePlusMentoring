using AutoMapper;
using DAL.Venues.Repository;
using MediatR;
using VenueApplication.Entities;
using VenueApplication.Queries;

namespace VenueApplication.Handlers
{
    public class GetVenueSectionsHandler : IRequestHandler<GetVenueSectionsQuery, IList<SectionDetails>>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IMapper _mapper;

        public GetVenueSectionsHandler(ISectionRepository sectionRepository,
          IVenueRepository venueRepository,
          IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _venueRepository = venueRepository;
            _mapper = mapper;
        }

        public async Task<IList<SectionDetails>> Handle(GetVenueSectionsQuery request, CancellationToken cancellationToken)
        {
            var venue = await _venueRepository.GetById(request.VenueId);
            if (venue == null)
              return null;

            var result = await _sectionRepository.GetByVenueId(request.VenueId);
            return _mapper.Map<IList<SectionDetails>>(result);
        }
    }
}