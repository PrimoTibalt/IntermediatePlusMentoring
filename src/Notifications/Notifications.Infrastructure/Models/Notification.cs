using ProtoBuf;

namespace API.Abstraction.Notifications
{
	[ProtoContract]
	public class Notification
	{
		[ProtoMember(1)]
		public Guid Id { get; set; }
		[ProtoMember(2)]
		public string Operation { get; set; }
		[ProtoMember(3)]
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		[ProtoMember(4)]
		public IDictionary<string, string> Parameters { get; set; }
		[ProtoMember(5)]
		public IDictionary<string, string> Content { get; set; }
	}
}
