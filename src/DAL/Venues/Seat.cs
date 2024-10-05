namespace DAL.Venues
{
	public sealed class Seat
	{
		public int Id { get; set; }
		public int RowId { get; set; }
		public Row Row { get; set; }
		public short Number { get; set; }
	}
}
