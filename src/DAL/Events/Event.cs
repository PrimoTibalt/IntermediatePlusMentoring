using DAL.Venues;
using ProtoBuf;

namespace DAL.Events
{
	[ProtoContract]
	public sealed class Event
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Name { get; set; }
		[ProtoMember(3)]
		public string Description { get; set; }
		[ProtoMember(4)]
		public int VenueId { get; set; }
		public Venue Venue { get; set; }
		[ProtoMember(5)]
		public DateTime StartDate { get; set; }
		[ProtoMember(6)]
		public DateTime EndDate { get; set; }
	}
}
