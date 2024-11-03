using System.Text.Json.Serialization;

namespace DAL.Events
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum SeatStatus
	{
		Available = 0,
		Booked,
		Sold
	}

	public static class SeatStatusStrings
	{
		public static string Available => SeatStatus.Available.ToString().ToLowerInvariant();
		public static string Booked => SeatStatus.Booked.ToString().ToLowerInvariant();
		public static string Sold => SeatStatus.Sold.ToString().ToLowerInvariant();
	}
}