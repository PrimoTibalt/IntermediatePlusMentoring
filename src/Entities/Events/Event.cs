﻿using Entities.Venues;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Events
{
	[Table("Events")]
	public sealed class Event
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int VenueId { get; set; }
		public Venue Venue { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
