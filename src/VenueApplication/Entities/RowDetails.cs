namespace VenueApplication.Entities
{
	public class RowDetails
	{
		public int Id { get; set; }
		public int SectionId { get; set; }
		public short Number { get; set; }
		public ICollection<int> Seats { get; set; }
	}
}
