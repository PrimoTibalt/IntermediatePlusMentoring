using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace TestsCore.Providers
{
    public static class LinkGeneratorMockObjectProvider
    {
        public static LinkGenerator Get()
        {
            var linkGenerator = new Mock<LinkGenerator>();
            linkGenerator.Setup(lg => lg.GetUriByAddress(
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
            return linkGenerator.Object;
        }
    }
}
