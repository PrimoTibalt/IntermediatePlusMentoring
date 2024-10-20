using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace TestsCore
{
	public static class LinkGeneratorMock
	{
		public static Mock<LinkGenerator> GetLinkGeneratorMock()
		{
			var linkGenerator = new Mock<LinkGenerator>();
			linkGenerator.Setup(lg => lg.GetUriByAddress<RouteValuesAddress>(
				It.IsAny<HttpContext>(),
				It.IsAny<RouteValuesAddress>(),
				It.IsAny<RouteValueDictionary>(),
				It.IsAny<RouteValueDictionary>(),
				It.IsAny<string>(),
				It.IsAny<HostString?>(),
				It.IsAny<PathString?>(),
				It.IsAny<FragmentString>(),
				It.IsAny<LinkOptions>()
				)).Returns(string.Empty);
			return linkGenerator;
		}
	}
}
