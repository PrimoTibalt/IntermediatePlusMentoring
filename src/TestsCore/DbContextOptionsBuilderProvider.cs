using Microsoft.EntityFrameworkCore;

namespace TestsCore
{
	public static class DbContextOptionsBuilderProvider<T> where T : DbContext
	{
		public static DbContextOptionsBuilder Get()
		{
			var builder = new DbContextOptionsBuilder<T>()
				.UseInMemoryDatabase(databaseName: nameof(T) + Guid.NewGuid().ToString());
			return builder;
		}
	}
}
