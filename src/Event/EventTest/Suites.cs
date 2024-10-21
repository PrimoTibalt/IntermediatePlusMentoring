using DAL.Events;
using EventApplication.Entities;
using TestsCore;

namespace EventTest
{
	internal class Suites
	{
		private static Dictionary<GetValuesSuites, List<Event>> suiteEventsMap => new()
		{
			{ GetValuesSuites.Empty, [] },
			{ GetValuesSuites.OneValue, [new()] },
			{ GetValuesSuites.ThreeValues, [new(),new(),new()] },
			{ GetValuesSuites.ManyValues, [new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),] },
		};

		private static Dictionary<GetValuesSuites, List<SeatDetails>> suiteSeatsMap => new()
		{
			{ GetValuesSuites.Empty, [] },
			{ GetValuesSuites.OneValue, [new()] },
			{ GetValuesSuites.ThreeValues, [new(),new(),new()] },
			{ GetValuesSuites.ManyValues, [new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),] }
		};

		public static IList<Event> GetEvents(GetValuesSuites suite)
		{
			return suiteEventsMap[suite];
		}

		public static IList<SeatDetails> GetSeats(GetValuesSuites suite)
		{
			return suiteSeatsMap[suite];
		}
	}
}
