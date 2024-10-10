namespace DAL.Venues.Repository
{
	public interface ISectionRepository : IGenericRepository<Section, int>
	{
		Task<IList<Section>> GetByVenueId(int venueId, CancellationToken token = default);
	}
}
