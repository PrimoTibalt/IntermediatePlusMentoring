namespace Entities.Notifications
{
	public class NotificationEntity
	{
		public Guid Id { get; set; }
		public int Status { get; set; }
		public byte[] Data { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
