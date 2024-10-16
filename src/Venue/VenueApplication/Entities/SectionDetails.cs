namespace VenueApplication.Entities
{
	public class SectionDetails
	{
		public int Id { get; set; }
		public int VenueId { get; set; }
		public string Name { get; set; }
		public ICollection<int> Rows { get; set; }
	}
}
