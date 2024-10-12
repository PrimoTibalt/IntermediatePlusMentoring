using System.Text.Json.Serialization;

namespace EventApplication.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SeatStatus
    {
        Available = 0,
        Booked,
        Sold
    }
}