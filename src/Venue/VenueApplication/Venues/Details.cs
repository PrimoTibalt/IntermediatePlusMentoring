using AutoMapper;
using DAL.Venues.Repository;
using MediatR;
using VenueApplication.Entities;

namespace VenueApplication.Venues
{
	public class Details
	{
		public class Query : IRequest<VenueDetails>
		{
			public int Id { get; set; }
		}

		public class RequestHandler : IRequestHandler<Query, VenueDetails>
		{
			private readonly IVenueRepository _repository;
			private readonly IMapper _mapper;

			public RequestHandler(IVenueRepository repository, IMapper mapper)
			{
				_repository = repository;
				_mapper = mapper;
			}

			public async Task<VenueDetails> Handle(Query request, CancellationToken cancellationToken)
			{
				var result = await _repository.GetDetailed(request.Id);
				return _mapper.Map<VenueDetails>(result);
			}
		}
	}
}
