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
}