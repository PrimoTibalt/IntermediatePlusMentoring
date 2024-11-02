using DAL.Events;
using ProtoBuf;

namespace EventApplication.Entities
{
	[ProtoContract]
	public class SeatDetails
	{
		[ProtoMember(1)]
		public long Id { get; set; }
		[ProtoMember(2)]
		public int RowId { get; set; }
		[ProtoMember(3)]
		public int SectionId { get; set; }
		[ProtoMember(4)]
		public int VenueSeatId { get; set; }
		[ProtoMember(5)]
		public SeatStatus Status { get; set; }
		[ProtoMember(6)]
		public Price Price { get; set; }
	}
}