using AutoMapper;
using EventApplication.Entities;
using EventApplication.Queries;
using EventApplication.Repositories;
using MediatR;

namespace EventApplication.Handlers
{
    public class GetEventSectionSeatsHandler : IRequestHandler<GetEventSectionSeatsQuery, IList<SeatDetails>>
    {
        private readonly IEventSeatRepository _repository;
        private readonly IMapper _mapper;

        public GetEventSectionSeatsHandler(IEventSeatRepository repository, IMapper mapper)
        {
            this._repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<SeatDetails>> Handle(GetEventSectionSeatsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetEventSectionSeats(request.EventId, request.SectionId);
            return _mapper.Map<IList<SeatDetails>>(result);
        }
    }
}