using DAL.Venues;
using DAL.Venues.Repository;
using MediatR;

namespace VenueApplication.Venues
{
	public class List 
	{
		public class Query : IRequest<IList<Venue>> { }

		public class ListHandler : IRequestHandler<Query, IList<Venue>>
		{
			private readonly IVenueRepository _repository;

			public ListHandler(IVenueRepository repository)
			{
				_repository = repository;
			}

			async Task<IList<Venue>> IRequestHandler<Query, IList<Venue>>.Handle(Query request, CancellationToken cancellationToken)
			{
				return await _repository.GetAll(cancellationToken);
			}
		}
	}
}
