using Kendo.Mvc.Examples.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kendo.Mvc.Examples.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKendoDemo(this IServiceCollection services)
        {
            foreach (var service in DemoServices.GetServices())
            {
                services.Add(service);
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
