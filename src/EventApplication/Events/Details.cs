using AutoMapper;
using DAL.Events.Repository;
using EventApplication.Entities;
using MediatR;

namespace EventApplication.Events
{
	public class Details
	{
		public class Query : IRequest<EventDetails>
		{
			public int Id { get; set; }
		}

		public class RequestHandler : IRequestHandler<Query, EventDetails>
		{
			private readonly IEventRepository _repository;
			private readonly IMapper _mapper;

			public RequestHandler(IEventRepository repository, IMapper mapper)
			{
				_repository = repository;
				_mapper = mapper;
			}

			public async Task<EventDetails> Handle(Query request, CancellationToken cancellationToken)
			{
				var result = await _repository.GetWithVenueAndSections(request.Id);
				return _mapper.Map<EventDetails>(result);
			}
		}
	}
}
