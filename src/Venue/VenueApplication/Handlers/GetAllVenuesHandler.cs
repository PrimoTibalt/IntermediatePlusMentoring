using Entities.Venues;
using MediatR;
using VenueApplication.Queries;
using VenueApplication.Repository;

namespace VenueApplication.Handlers
{
    public class GetAllVenuesHandler : IRequestHandler<GetAllVenuesQuery, IList<Venue>>
    {
        private readonly IVenueRepository _repository;

        public GetAllVenuesHandler(IVenueRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<Venue>> Handle(GetAllVenuesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll(cancellationToken);
        }
    }
}