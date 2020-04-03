using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Kendo.Mvc.Examples
{
	public class HyphenatedRouteHandler : IRouter
	{
		private readonly IRouter _target;
        public HyphenatedRouteHandler(IRouter target)
		{
			_target = target;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
		{
			return null;
		}

		public async Task RouteAsync(RouteContext context)
		{
			context.RouteData.Values["controller"] = context.RouteData.Values["controller"].ToString().Replace("-", "_");

			context.RouteData.Values["action"] = context.RouteData.Values["action"].ToString().Replace("-", "_");

			await _target.RouteAsync(context);
		}
	}	
}