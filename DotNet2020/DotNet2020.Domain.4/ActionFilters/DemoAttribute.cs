//using Kendo.Mvc.Examples.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Kendo.Mvc.Examples.Extensions;

//namespace Kendo.Mvc.Examples.Controllers
//{
//    public class DemoAttribute : ActionFilterAttributeBase
//    {
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            base.OnActionExecuting(filterContext);

//            FindCurrentExample();

//            NavigationExample currentExample = ViewBag.CurrentExample;
//            NavigationWidget currentWidget = ViewBag.CurrentWidget;

//            if (currentWidget == null)
//            {
//                return;
//            }

//            ViewBag.Description = Description(ViewBag.Product, currentExample, currentWidget);

//            var exampleFiles = new List<ExampleFile>();
//            exampleFiles.AddRange(SourceCode());
//            exampleFiles.AddRange(AdditionalSources(currentWidget.Sources));
//            exampleFiles.AddRange(AdditionalSources(currentExample.Sources));
//            ViewBag.ExampleFiles = exampleFiles.Where(file => file.Exists(ContentRootPath));

//            var api = currentExample.Api ?? ViewBag.CurrentWidget.Api;
//            if (!string.IsNullOrEmpty(api))
//            {
//                if (api == "web/validator")
//                {
//                    ViewBag.Api = "http://docs.telerik.com/kendo-ui/aspnet-mvc/validation";
//                }
//                else
//                {
//                    ViewBag.Api = IUrlHelperExtensions.IsAbsoluteUrl(api) ? api : "http://docs.telerik.com/aspnet-core/api" + Regex.Replace(api, "(web|dataviz|framework)", "").Replace("mobile/", "/mobile");
//                }
//            }

//            if (currentWidget.Documentation != null && currentWidget.Documentation.ContainsKey(ViewBag.Product))
//            {
//                var documentationLink = currentWidget.Documentation[ViewBag.Product];
//                ViewBag.Documentation = IUrlHelperExtensions.IsAbsoluteUrl(documentationLink) ? documentationLink : "http://docs.telerik.com/aspnet-core/" + documentationLink;
//            }

//            if (currentWidget.Forum != null && currentWidget.Forum.ContainsKey(ViewBag.Product))
//            {
//                ViewBag.Forum = "http://www.telerik.com/forums/" + currentWidget.Forum[ViewBag.Product];
//            }
//        }

//        protected void FindCurrentExample()
//        {
//            var found = false;

//            NavigationExample current = null;
//            NavigationWidget currentWidget = null;

//            foreach (NavigationWidget widget in ViewBag.Navigation)
//            {
//                foreach (NavigationExample example in widget.Items)
//                {
//                    if (example.ShouldInclude())
//                    {
//                        examplesUrl.Add("~/" + example.Url);
//                    }

//                    if (!found && IsCurrentExample(example.Url))
//                    {
//                        current = example;
//                        currentWidget = widget;
//                        found = true;
//                    }
//                }
//            }

//            ViewBag.CurrentWidget = currentWidget;

//            if (currentWidget == null)
//            {
//                return;
//            }

//            ViewBag.CurrentExample = current;

//            if (current.Title != null)
//            {
//                if (current.Title.ContainsKey(MvcFlavor.AspNetCore))
//                {
//                    ViewBag.Title = current.Title[MvcFlavor.AspNetCore];
//                }
//            }
//            else
//            {
//                ViewBag.Title = current.Text;
//            }

//            if (current.Meta != null)
//            {
//                if (current.Meta.ContainsKey(MvcFlavor.AspNetCore))
//                {
//                    ViewBag.Meta = current.Meta[MvcFlavor.AspNetCore];
//                }
//            }
//        }

//        private bool IsCurrentExample(string url)
//        {
//            var section = Controller.ControllerContext.RouteData.Values["controller"].ToString().ToLower().Replace("_", "-");
//            var example = Controller.ControllerContext.RouteData.Values["action"].ToString().ToLower().Replace("_", "-");

//            var components = url.Split('/');

//            return (section == components[0] && example == components[1]) ||
//                (section == "upload" && example.Contains("submit") && components[0] == "upload" && components[1] == "index");
//        }

//        protected string Description(string product, NavigationExample example, NavigationWidget widget)
//        {
//            if (example.Description != null && example.Description.ContainsKey(product))
//            {
//                return example.Description[product];
//            }
//            else if (widget.Description != null && widget.Description.ContainsKey(product))
//            {
//                return widget.Description[product];
//            }

//            return null;
//        }

//        protected IEnumerable<ExampleFile> SourceCode()
//        {
//            var section = Controller.ControllerContext.RouteData.Values["controller"].ToString().ToLower().Replace("_", "-");
//            var example = Controller.ControllerContext.RouteData.Values["action"].ToString().ToLower().Replace("_", "-");

//            IFrameworkDescription framework = new AspNetCoreDescription();

//            return framework.GetFiles(ContentRootPath, example, section);
//        }

//        protected IEnumerable<ExampleFile> AdditionalSources(IDictionary<string, IEnumerable<ExampleFile>> sources)
//        {
//            var files = new List<ExampleFile>();

//            if (sources != null && sources.ContainsKey(MvcFlavor.AspNetCore))
//            {
//                files.AddRange(sources[MvcFlavor.AspNetCore]);
//            }

//            return files;
//        }

//    }
//}