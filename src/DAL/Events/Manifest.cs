namespace DAL.Events
{
	public sealed class Manifest
	{
		public int Id { get; set; }
		public int VenueId { get; set; }
		public Venue.Venue Venue { get; set; }
	}
}
