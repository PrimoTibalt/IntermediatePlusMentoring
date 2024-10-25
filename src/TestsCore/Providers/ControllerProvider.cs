using MediatR;
using Microsoft.AspNetCore.Mvc;

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
	}
}
