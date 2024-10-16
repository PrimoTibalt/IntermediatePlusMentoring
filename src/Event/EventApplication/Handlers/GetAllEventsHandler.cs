using DAL.Events;
using DAL.Events.Repository;
using EventApplication.Queries;
using MediatR;

namespace EventApplication.Handlers
{
    public class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, IList<Event>>
	{
		private readonly IEventRepository _repository;

		public GetAllEventsHandler(IEventRepository repository)
		{
			_repository = repository;
		}

		public async Task<IList<Event>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
		{
			return await _repository.GetAll();
		}
	}
}