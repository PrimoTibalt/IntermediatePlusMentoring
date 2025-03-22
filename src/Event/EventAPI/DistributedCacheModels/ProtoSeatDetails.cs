using Entities.Events;
using EventApplication.Entities;
using ProtoBuf;

namespace EventAPI.DistributedCacheModels
{
	[ProtoContract]
	public sealed class ProtoSeatDetails
	{
		private ProtoSeatDetails() { }
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
		public ProtoPrice Price { get; set; }
		public static ProtoSeatDetails Create(SeatDetails model)
		{
			var entity = new ProtoSeatDetails();
			entity.Id = model.Id;
			entity.RowId = model.RowId;
			entity.SectionId = model.SectionId;
			entity.VenueSeatId = model.VenueSeatId;
			entity.Status = model.Status;
			entity.Price = ProtoPrice.Create(model.Price);
			return entity;
		}
	}
}
