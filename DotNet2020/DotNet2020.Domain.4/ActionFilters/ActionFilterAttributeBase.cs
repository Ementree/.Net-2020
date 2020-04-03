//using Kendo.Mvc.Examples.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using IOFile = System.IO.File;

//namespace Kendo.Mvc.Examples.Controllers
//{
//    public abstract class ActionFilterAttributeBase : ActionFilterAttribute
//    {
//        protected static string Header = "";
//        protected static string Footer = "";
//        protected static bool ResetHeader = true;
//        protected static bool ResetFooter = true;
//        protected static string TelerikNavigationVersion = "stable";
//        private IHostingEnvironment _hostingEnvironment;

//        protected List<string> examplesUrl = new List<string>();

//        public dynamic ViewBag
//        {
//            get
//            {
//                return Controller.ViewBag;
//            }
//        }

//        public Controller Controller { get; set; }

//        public string ContentRootPath { get; set; }

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            _hostingEnvironment = filterContext.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>();

//            ContentRootPath = _hostingEnvironment.ContentRootPath;

//            Controller = filterContext.Controller as Controller;
//            string Url = filterContext.HttpContext.Request.Host.Value + filterContext.HttpContext.Request.Path.Value;

//            if (ResetHeader)
//            {
//                UpdateHeader(Url);
//            }
//            if (ResetFooter)
//            {
//                UpdateFooter(Url);
//            }

//            ViewBag.ShowCodeStrip = true;
//            ViewBag.Product = MvcFlavor.AspNetCore;
//            ViewBag.TelerikNavigationHeader = Header;
//            ViewBag.TelerikNavigationFooter = Footer;
//            ViewBag.NavProduct = "mvc-core";

//            ViewBag.Theme = "default-v2";
//            ViewBag.CommonFile = "common-empty";

//            SetTheme();
//            LoadNavigation();
//            LoadCategories();

//            if (Url.IndexOf("updateteleriknavigation") != -1)
//            {
//                ResetHeader = true;
//                ResetFooter = true;
//            }
//        }

//        protected void SetTheme()
//        {
//            var theme = "default-v2";
//            var themeParam = Controller.HttpContext.Request.Query["theme"].FirstOrDefault();
//            var themeCookie = Controller.HttpContext.Request.Cookies["theme"];

//            if (themeParam != null && Regex.IsMatch(themeParam, "[a-z0-9\\-]+", RegexOptions.IgnoreCase))
//            {
//                theme = themeParam;

//                // update cookie
//                Controller.ControllerContext.HttpContext.Response.Cookies.Append("theme", theme);
//            }
//            else if (!string.IsNullOrEmpty(themeCookie))
//            {
//                theme = themeCookie;
//            }

//            var CommonFileCookie = Controller.HttpContext.Request.Cookies["commonFile"];

//            ViewBag.Theme = theme;
//            ViewBag.CommonFile = string.IsNullOrEmpty(CommonFileCookie) ? "common-empty" : CommonFileCookie;
//        }

//        protected void LoadNavigation()
//        {
//            ViewBag.Navigation = LoadWidgets();
//        }

//        protected void LoadCategories()
//        {
//            ViewBag.WidgetCategories = LoadWidgets().GroupBy(w => w.Category).ToList();
//        }

//        protected IEnumerable<NavigationWidget> LoadWidgets()
//        {
//            var rootPath = _hostingEnvironment.WebRootPath;
//            var navJson = IOFile.ReadAllText(rootPath + "/shared/nav.json");

//            return JsonConvert.DeserializeObject<NavigationWidget[]>(navJson.Replace("$FRAMEWORK", "ASP.NET Core"));
//        }

//        protected void UpdateHeader(string Url)
//        {
//            ResetHeader = false;

//            if (Url.IndexOf("localhost") == -1)
//            {
//                string ProductName = "asp-net-core";
//                string cdnEnvironment = "";

//                if (Url.IndexOf("kendobuild") != -1)
//                {
//                    cdnEnvironment = "uat";
//                }

//                string urlAddress = "https://" + cdnEnvironment + "cdn.telerik-web-assets.com/telerik-navigation/" + TelerikNavigationVersion + "/nav-" + ProductName + "-csa-abs-component.html";

//                try
//                {
//                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
//                    request.Timeout = 4000;
//                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//                    {
//                        if (response.StatusCode == HttpStatusCode.OK)
//                        {
//                            Stream receiveStream = response.GetResponseStream();
//                            StreamReader readStream = null;

//                            if (response.CharacterSet == null)
//                            {
//                                readStream = new StreamReader(receiveStream);
//                            }
//                            else
//                            {
//                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                            }

//                            string data = readStream.ReadToEnd();

//                            Header = data;

//                            readStream.Close();
//                            receiveStream.Close();
//                            response.Close();
//                        }
//                    }
//                }
//                catch (Exception) { }
//            }
//        }

//        protected void UpdateFooter(string Url)
//        {
//            ResetFooter = false;

//            if (Url.IndexOf("localhost") == -1)
//            {
//                string cdnEnvironment = "";

//                if (Url.IndexOf("kendobuild") != -1)
//                {
//                    cdnEnvironment = "uat";
//                }

//                string urlAddress = "https://" + cdnEnvironment + "cdn.telerik-web-assets.com/telerik-navigation/" + TelerikNavigationVersion + "/footer-big-abs-markup.html";

//                try
//                {
//                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
//                    request.Timeout = 4000;
//                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//                    {
//                        if (response.StatusCode == HttpStatusCode.OK)
//                        {
//                            Stream receiveStream = response.GetResponseStream();
//                            StreamReader readStream = null;

//                            if (response.CharacterSet == null)
//                            {
//                                readStream = new StreamReader(receiveStream);
//                            }
//                            else
//                            {
//                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                            }

//                            string data = readStream.ReadToEnd();

//                            Footer = data;

//                            readStream.Close();
//                            receiveStream.Close();
//                            response.Close();
//                        }
//                    }
//                }
//                catch (Exception) { }
//            }
//        }
//    }
//}
