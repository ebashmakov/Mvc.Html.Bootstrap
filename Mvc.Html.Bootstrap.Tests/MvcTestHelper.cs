﻿using Moq;
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
            return GetHtmlHelper(new ViewDataDictionary(), out writer);
        }

        public static HtmlHelper<object> GetHtmlHelper(ViewDataDictionary viewData)
        {
            StringWriter writer;
            return GetHtmlHelper(viewData, out writer);
        }

        public static HtmlHelper<object> GetHtmlHelper(out StringWriter writer)
        {
            return GetHtmlHelper(new ViewDataDictionary(), out writer);
        }

        public static HtmlHelper<object> GetHtmlHelper(ViewDataDictionary viewData, out StringWriter writer)
        {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData).Returns(viewData);

            writer = new StringWriter();
            var controllerContext = new ControllerContext(httpcontext, rd, new Mock<ControllerBase>().Object);
            var mockViewContext = new Mock<ViewContext>(controllerContext, new Mock<IView>().Object, viewData, new TempDataDictionary(), TextWriter.Null);
            mockViewContext.Setup(c => c.Writer).Returns(writer);

            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(viewData);

            HtmlHelper<object> htmlHelper = new HtmlHelper<object>(mockViewContext.Object, mockVdc.Object, rt);
            return htmlHelper;
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

            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "index");

            mockViewContext.Setup(c => c.RouteData).Returns(rd);
            HtmlHelper helper = new HtmlHelper(mockViewContext.Object, new Mock<IViewDataContainer>().Object, rt);
            //helper.ViewContext.FormIdGenerator = () => "form_id";
            return helper;
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
