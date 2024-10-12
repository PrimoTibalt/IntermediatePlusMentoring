using AutoMapper;
using DAL.Events.Repository;
using EventApplication.Entities;
using MediatR;

namespace EventApplication.Seats;

public class List
{
    public class Query : IRequest<IList<SeatDetails>> 
    {
        public int EventId { get; set; }
        public int SectionId { get; set; }
    }

    public class RequestHandler : IRequestHandler<Query, IList<SeatDetails>>
    {
        private readonly IEventSeatRepository _repository;
        private readonly IMapper _mapper;

        public RequestHandler(IEventSeatRepository repository, IMapper mapper)
        {
            this._repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<SeatDetails>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetEventSectionSeats(request.EventId, request.SectionId);
            return _mapper.Map<IList<SeatDetails>>(result);
        }
    }
}