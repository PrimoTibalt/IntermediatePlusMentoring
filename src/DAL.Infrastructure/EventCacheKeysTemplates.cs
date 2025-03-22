namespace Cache.Infrastructure
{
	public static class EventCacheKeysTemplates
	{
		private const string GetAllSelector = "*";
		private const string EventAppCacheName = "Event.";
		private const string EventSeatEntityCacheName = "EventSeats.";

		public const string EventAppCacheTemplate = EventAppCacheName + GetAllSelector;
		public static readonly string AllEventsCacheKey = EventAppCacheTemplate.Replace(GetAllSelector, "AllEvents");
		public const string EventAppEventSeatsTemplate = EventAppCacheName + EventSeatEntityCacheName + GetAllSelector;
		public static readonly string EventAppEventSeatsByEventIdSectionIdCacheTemplate = EventAppEventSeatsTemplate.Replace(GetAllSelector, "EventId={0}.SectionId={1}");
	}
}
