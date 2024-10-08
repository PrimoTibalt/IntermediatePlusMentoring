using Microsoft.EntityFrameworkCore;

namespace DAL.Venues
{
	public sealed class VenueContext : DbContext
	{
		public VenueContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Venue> Venues { get; set; }
		public DbSet<Section> Sections { get; set; }
		public DbSet<Row> Rows { get; set; }
		public DbSet<Seat> Seats { get; set; }
	}
}
