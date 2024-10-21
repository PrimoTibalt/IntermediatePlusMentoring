using DAL.Venues;
using TestsCore;

namespace VenueTests
{
	public static class Suites
	{
		private static Dictionary<GetValuesSuites, List<Venue>> suiteVenuesMap => new ()
		{
			{ GetValuesSuites.Empty, new List<Venue>() },
			{ GetValuesSuites.OneValue, new List<Venue> { new() { } } },
			{ GetValuesSuites.ThreeValues, new List<Venue>
				{
					new Venue { },
					new Venue { },
					new Venue { },
				}
			},
			{ GetValuesSuites.ManyValues, new List<Venue>{new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),new()}
			}
		};

		private static Dictionary<GetValuesSuites, List<Section>> suiteSectionsMap => new()
		{
			{ GetValuesSuites.Empty, new List<Section>() },
			{ GetValuesSuites.Null, null },
			{ GetValuesSuites.OneValue, new() { new() } },
			{ GetValuesSuites.ThreeValues, new() { new(), new(), new() } },
		};

		public static IList<Venue> GetVenues(GetValuesSuites suite)
		{
			return suiteVenuesMap[suite];
		}

		public static IList<Section> GetSections(GetValuesSuites suite)
		{
			return suiteSectionsMap[suite];
		}
	}
}
