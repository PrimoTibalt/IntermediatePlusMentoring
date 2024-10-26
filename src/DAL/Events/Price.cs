using ProtoBuf;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Events
{
	[ProtoContract]
	public sealed class Price
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Type { get; set; }
		[Column("Price")]
		[ProtoMember(3)]
		public decimal Sum { get; set; }
	}
}
