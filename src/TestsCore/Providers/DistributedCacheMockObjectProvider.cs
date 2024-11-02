using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace TestsCore.Providers
{
	public static class DistributedCacheMockObjectProvider
	{
		public static IDistributedCache Get()
		{
			var cache = new Mock<IDistributedCache>();
			cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((byte[])null);
			cache.Setup(c => c.SetAsync(It.IsAny<string>(),
				It.IsAny<byte[]>(),
				It.IsAny<DistributedCacheEntryOptions>(),
				It.IsAny<CancellationToken>()));
			return cache.Object;
		}
	}
}
