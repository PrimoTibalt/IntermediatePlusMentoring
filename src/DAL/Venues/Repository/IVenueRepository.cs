namespace DAL.Venues.Repository
{
	public interface IVenueRepository : IGenericRepository<Venue, int>
	{
		Task<Venue> GetDetailed(int id);
	}
}
