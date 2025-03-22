using Entities.Events;
using ProtoBuf;

namespace EventAPI.DistributedCacheModels
{
	[ProtoContract]
	public sealed class ProtoPrice
	{
		private ProtoPrice() { }
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Type { get; set; }
		[ProtoMember(3)]
		public decimal Sum { get; set; }
		public static ProtoPrice Create(Price model)
		{
			var entity = new ProtoPrice();
			entity.Id = model.Id;
			entity.Type = model.Type;
			entity.Sum = model.Sum;
			return entity;
		}
	}
}