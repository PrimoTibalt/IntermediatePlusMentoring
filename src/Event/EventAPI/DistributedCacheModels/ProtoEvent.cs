using Entities.Events;
using ProtoBuf;

namespace EventAPI.DistributedCacheModels
{
	[ProtoContract]
	public sealed class ProtoEvent
	{
		private ProtoEvent() {}

		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]	
		public string Name { get; set; }
		[ProtoMember(3)]
		public string Description { get; set; }
		[ProtoMember(4)]
		public int VenueId { get; set; }
		[ProtoMember(5)]
		public DateTime StartDate { get; set; }
		[ProtoMember(6)]
		public DateTime EndDate { get; set; }

		public static ProtoEvent Create(Event model)
		{
			var entity = new ProtoEvent();
			entity.Id = model.Id;
			entity.Name = model.Name;
			entity.Description = model.Description;
			entity.VenueId = model.VenueId;
			entity.StartDate = model.StartDate;
			entity.EndDate = model.EndDate;
			return entity;
		}
	}
}
