namespace DAL.Venues
{
	public sealed class Section
	{
		public int Id { get; set; }
		public int VenueId { get; set; }
		public Venue Venue { get; set; }
		public string Name { get; set; }
		public ICollection<Row> Rows { get; set; }
	}
}
