namespace EventApplication.Entities
{
	public class EventDetails
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string VenueAddress { get; set; }
		public string VenueName { get; set; }
		public string VenueDescription { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public IList<int> Sections { get; set; }
	}
}
