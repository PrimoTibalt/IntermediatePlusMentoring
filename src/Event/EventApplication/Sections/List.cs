using AutoMapper;
using DAL.Events.Repository;
using EventApplication.Entities;
using MediatR;

namespace EventApplication.Sections;

public class List
{
	public class Query : IRequest<IList<SectionDetails>>
	{
		public int Id { get; set; }
	}

	public class RequestHandler : IRequestHandler<Query, IList<SectionDetails>>
	{
		private readonly IEventRepository _repository;
		private readonly IMapper _mapper;

		public RequestHandler(IEventRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IList<SectionDetails>> Handle(Query request, CancellationToken cancellationToken)
		{
			return _mapper.Map<IList<SectionDetails>>(await _repository.GetSections(request.Id));
		}
	}
}
