using Entities.Events;
using Entities.Venues;
using Microsoft.EntityFrameworkCore;

namespace DAL.Events
{
	public sealed class EventContext : DbContext
	{
		public EventContext(DbContextOptions options) : base(options) { }

		public DbSet<Event> Events { get; set; }
		public DbSet<EventSeat> EventSeats { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<Venue> Venues { get; set; }
		public DbSet<Seat> Seats { get; set; }
		public DbSet<Section> Sections { get; set; }
		public DbSet<Row> Rows { get; set; }
	}
}
