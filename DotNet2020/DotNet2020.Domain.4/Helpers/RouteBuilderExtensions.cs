using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Kendo.Mvc.Examples
{
	public static class RouteBuilderExtensions {
        public static IRouteBuilder AddHyphenatedRoute(this IRouteBuilder routeBuilder)
        {
            var constraintResolver = routeBuilder.ServiceProvider.GetService<IInlineConstraintResolver>();

            routeBuilder.Routes.Add(new Route(
                new HyphenatedRouteHandler(routeBuilder.DefaultHandler),
                "{controller}/{action}/{id?}",
                constraintResolver)
            );

            return routeBuilder;
        }
    }
}