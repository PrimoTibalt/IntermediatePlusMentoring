using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace TestsCore.Providers
{
	public static class ControllerProvider
	{
		public static TController Get<TController>(IMediator mediator, bool hasLinkGenerator = true) where TController : ControllerBase
		{
			var controller = hasLinkGenerator ?
				(TController)Activator.CreateInstance(typeof(TController), mediator, LinkGeneratorMockObjectProvider.Get())
				: (TController)Activator.CreateInstance(typeof(TController), mediator);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}

		public static TController GetWithCache<TController>(IMediator mediator, IDistributedCache cache, bool hasLinkGenerator = true) where TController : ControllerBase
		{
			var controller = hasLinkGenerator ?
				(TController)Activator.CreateInstance(typeof(TController), mediator, LinkGeneratorMockObjectProvider.Get(), cache)
				: (TController)Activator.CreateInstance(typeof(TController), mediator, cache);
			var controllerContext = ControllerContextProvider.GetControllerContext();
			controller.ControllerContext = controllerContext;
			return controller;
		}
	}
}
