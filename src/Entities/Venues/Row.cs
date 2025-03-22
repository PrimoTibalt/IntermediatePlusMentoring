namespace Entities.Venues
{
	public sealed class Row
	{
		public int Id { get; set; }
		public int SectionId { get; set; }
		public Section Section { get; set; }
		public short Number { get; set; }
		public ICollection<Seat> Seats { get; set; }
	}
}
