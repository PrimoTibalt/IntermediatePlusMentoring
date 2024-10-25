namespace TestsCore
{
	public static class ListGenerator
	{
		public static IList<T> Generate<T>(GetValuesSuites suite) where T : new()
		{
			switch (suite)
			{
				case GetValuesSuites.Null:
					return null;
				case GetValuesSuites.Empty:
					return Array.Empty<T>();
				case GetValuesSuites.OneValue:
					return [new()];
				case GetValuesSuites.ManyValues:
					return Enumerable.Range(0, 1000).Select(_ => new T()).ToList();
				default:
					throw new NotImplementedException($"Unknown suite '{suite}'.");
			}
		}
	}
}
