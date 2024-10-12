using DAL.Events;
using DAL.Events.Repository;
using MediatR;

namespace EventApplication.Events
{
	public class List
	{
		public class Query : IRequest<IList<Event>> { }

		public class RequestHandler : IRequestHandler<Query, IList<Event>>
		{
			private readonly IEventRepository _repository;

			public RequestHandler(IEventRepository repository)
			{
				_repository = repository;
			}

			public async Task<IList<Event>> Handle(Query request, CancellationToken cancellationToken)
			{
				return await _repository.GetAll();
			}
		}
	}
}
