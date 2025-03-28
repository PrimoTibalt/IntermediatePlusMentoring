﻿namespace Entities.Venues
{
	public sealed class Venue
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }
		public ICollection<Section> Sections { get; set; }
	}
}
