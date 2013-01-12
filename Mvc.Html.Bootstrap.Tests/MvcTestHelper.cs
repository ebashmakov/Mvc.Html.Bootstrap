using Moq;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc.Html.Bootstrap.Tests
{
    public static class MvcTestHelper
    {
        public const string AppPathModifier = "/$(SESSION)";

        public static HtmlHelper<object> GetHtmlHelper()
        {
            StringWriter writer;
            return GetHtmlHelper<object>(new ViewDataDictionary(), out writer);
        }

        public static HtmlHelper<object> GetHtmlHelper(ViewDataDictionary viewData)
        {
            StringWriter writer;
            return GetHtmlHelper<object>(viewData, out writer);
        }

        public static HtmlHelper<object> GetHtmlHelper(out StringWriter writer)
        {
            return GetHtmlHelper<object>(new ViewDataDictionary(), out writer);
        }

        public static HtmlHelper<T> GetHtmlHelper<T>(ViewDataDictionary viewData)
        {
            StringWriter writer;
            return GetHtmlHelper<T>(viewData, out writer);
        }

        public static HtmlHelper<T> GetHtmlHelper<T>(ViewDataDictionary viewData, out StringWriter writer)
        {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = GetRouteCollection();
            RouteData rd = GetRouteData();
            writer = new StringWriter();
            
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.HttpContext).Returns(httpcontext);
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.RouteData).Returns(rd);
            mockViewContext.Setup(c => c.TempData).Returns(new TempDataDictionary());
            mockViewContext.Setup(c => c.Writer).Returns(writer);

            var htmlHelper = new HtmlHelper<T>(mockViewContext.Object, GetViewDataContainer(viewData), rt);
            return htmlHelper;
        }

        private static RouteData GetRouteData()
        {
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "index");
            return rd;
        }

        private static RouteCollection GetRouteCollection()
        {
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            return rt;
        }

        public static HtmlHelper GetFormHelper(out StringWriter writer)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.HttpContext.Request.Url).Returns(new Uri("http://www.contoso.com/some/path"));
            mockViewContext.Setup(c => c.HttpContext.Request.RawUrl).Returns("/some/path");
            mockViewContext.Setup(c => c.HttpContext.Request.ApplicationPath).Returns("/");
            mockViewContext.Setup(c => c.HttpContext.Request.Path).Returns("/");
            mockViewContext.Setup(c => c.HttpContext.Request.ServerVariables).Returns((NameValueCollection)null);
            mockViewContext.Setup(c => c.HttpContext.Response.Write(It.IsAny<string>())).Throws(new Exception("Should not be called"));
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());

            writer = new StringWriter();
            mockViewContext.Setup(c => c.Writer).Returns(writer);
            mockViewContext.Setup(c => c.HttpContext.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockViewContext.Setup(c => c.TempData).Returns(new TempDataDictionary());
            RouteCollection rt = GetRouteCollection();
            RouteData rd = GetRouteData();

            mockViewContext.Setup(c => c.RouteData).Returns(rd);
            HtmlHelper helper = new HtmlHelper(mockViewContext.Object, GetViewDataContainer(new ViewDataDictionary()), rt);
            //helper.ViewContext.FormIdGenerator = () => "form_id";
            return helper;
        }

        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(viewData);
            return mockVdc.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port)
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            if (!String.IsNullOrEmpty(appPath))
            {
                mockHttpContext.Setup(o => o.Request.ApplicationPath).Returns(appPath);
                mockHttpContext.Setup(o => o.Request.RawUrl).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath))
            {
                mockHttpContext.Setup(o => o.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri;

            if (port >= 0)
            {
                uri = new Uri(protocol + "://localhost" + ":" + Convert.ToString(port));
            }
            else
            {
                uri = new Uri(protocol + "://localhost");
            }
            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);

            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod))
            {
                mockHttpContext.Setup(o => o.Request.HttpMethod).Returns(httpMethod);
            }

            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod)
        {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1);
        }
    }
}
