using System.Text.Json.Serialization;

namespace Entities.Events
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum SeatStatus
	{
		Available = 0,
		Booked,
		Sold
	}
}