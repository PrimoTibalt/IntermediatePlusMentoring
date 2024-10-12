using AutoMapper;
using DAL.Events.Repository;
using DAL.Venues;
using MediatR;

namespace EventApplication.Events
{
	public class ListSections
	{
		public class Query : IRequest<IList<Section>>
		{
			public int Id { get; set; }
		}

		public class RequestHandler : IRequestHandler<Query, IList<Section>>
		{
			private readonly IEventRepository _repository;
			private readonly IMapper _mapper;

			public RequestHandler(IEventRepository repository, IMapper mapper)
			{
				_repository = repository;
				_mapper = mapper;
			}
			public async Task<IList<Section>> Handle(Query request, CancellationToken cancellationToken)
			{
				return await _repository.GetSections(request.Id);
			}
		}
	}
}
