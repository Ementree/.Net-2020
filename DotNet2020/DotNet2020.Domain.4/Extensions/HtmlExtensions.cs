using Kendo.Mvc.Examples.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace Kendo.Mvc.Examples.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlContent ExampleLink(this IHtmlHelper html, NavigationExample example)
        {
            var href = html.ExampleUrl(example);

            var className = "";

            if (example.New)
            {
                className += "new-example";
            }

            if (example.Updated)
            {
                className += "updated-example";
            }

            var routeData = html.ViewContext.RouteData;
            var currentAction = routeData.Values["action"].ToString().ToLower().Replace("_", "-");
            var currentController = routeData.Values["controller"].ToString().ToLower().Replace("_", "-");

            if (href.EndsWith(currentController + "/" + currentAction))
            {
                className += " active";
            }

            StringBuilder link = new StringBuilder();

            link.Append("<a ");

            if (!String.IsNullOrEmpty(className))
            {
                link.Append("class=\"" + className + "\" ");
            }

            link.Append("href=\"" + href + "\">");

            if (example.New)
            {
                link.Append("<span class=\"new-widget\"></span>");
            }

            if (example.Updated)
            {
                link.Append("<span class=\"updated-widget\"></span>");
            }

            link.Append(example.Text).Append("</a>");

            return html.Raw(link.ToString());
        }

        public static string ExampleUrl(this IHtmlHelper html, NavigationExample example)
        {
            var sectionAndExample = example.Url.Split('/');

            return new UrlHelper(html.ViewContext).ExampleUrl(sectionAndExample[0], sectionAndExample[1]);
        }

        public static string ExampleUrl(this IHtmlHelper html, NavigationExample example, string product)
        {
            var sectionAndExample = example.Url.Split('/');

            var url = string.Join("/", LiveSamplesRoot, product, sectionAndExample[0], sectionAndExample[1]);

            return url;
        }

        public static string ProductExampleUrl(this IHtmlHelper html, NavigationExample example, string product)
        {
            var sectionAndExample = example.Url.Split('/');

            var url = string.Join("/", LiveSamplesRoot, product, sectionAndExample[0]);

            return url;
        }

        public static string LiveSamplesRoot
        {
            get
            {
                return "http://demos.telerik.com";
            }
        }

        public static IHtmlContent WidgetLink(this IHtmlHelper html, NavigationWidget widget, string product)
        {
            var href = html.ExampleUrl(widget.Items[0]);

            var text = widget.Text;

            StringBuilder link = new StringBuilder();

            link.AppendFormat("<a href=\"{0}\">", href);
            link.Append(text);

            if (widget.Beta)
            {
                link.Append("<span class=\"beta-widget\"></span>");
            }

            if (widget.New)
            {
                link.Append("<span class=\"new-widget\"></span>");
            }

            if (widget.Updated)
            {
                link.Append("<span class=\"updated-widget\"></span>");
            }

            link.Append("</a>");

            return html.Raw(link.ToString());
        }

        public static string StyleRel(this IHtmlHelper html, string styleName)
        {
            if (styleName.ToLowerInvariant().EndsWith("less"))
            {
                return "stylesheet/less";
            }

            return "stylesheet";
        }

        public static IHtmlContent StyleLink(this IHtmlHelper html, string styleName)
        {
            var urlHelper = new UrlHelper(html.ViewContext);
            var url = urlHelper.Style(styleName);
            return html.Raw("<link href=\"" + url + "\" rel=\"" + html.StyleRel(styleName) + "\" />");
        }

        public static IHtmlContent StyleLink(this IHtmlHelper html, string styleName, string theme, string common)
        {
            var urlHelper = new UrlHelper(html.ViewContext);
            var disabled = "";
            if (common == "common-empty" && (
                  styleName.Contains("kendo.rtl") ||
                  styleName.Contains("CURRENT_COMMON") ||
                  styleName.Contains("CURRENT_THEME.mobile"))) {
              disabled = "-disabled";
            }
            var url = urlHelper.Style(styleName, theme, common);
            return html.Raw("<link href=\"" + url + "\" rel=\"" + html.StyleRel(styleName) + disabled + "\" />");
        }
    }
}
