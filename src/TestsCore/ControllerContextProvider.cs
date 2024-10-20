using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace TestsCore
{
	public static class ControllerContextProvider
	{
		public static ControllerContext GetControllerContext()
		{
			var actionContext = new ActionContext
			{
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData(),
				ActionDescriptor = new ControllerActionDescriptor()
			};
			return new ControllerContext(actionContext);
		}
	}
}
