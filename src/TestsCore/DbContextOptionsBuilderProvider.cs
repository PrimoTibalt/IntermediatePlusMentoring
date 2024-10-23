using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TestsCore
{
	public static class DbContextOptionsBuilderProvider<T> where T : DbContext
	{
		public static DbContextOptionsBuilder Get()
		{
			var builder = new DbContextOptionsBuilder<T>()
				.UseInMemoryDatabase(databaseName: nameof(T) + Guid.NewGuid().ToString())
				.ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));
			return builder;
		}
	}
}
