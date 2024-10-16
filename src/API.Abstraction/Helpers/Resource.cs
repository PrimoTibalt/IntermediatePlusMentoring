namespace API.Abstraction.Helpers
{
	public class Resource<T>
	{
		public T Value { get; set; }
		public IList<Link> Links { get; set; }
	}

	public class Link
	{
		public string Href { get; set; }
		public string Method { get; set; }
	}
}
