using System.IO;
using System.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;

namespace Mvc.Html.Bootstrap.Tests
{
    public static class HtmlHelperTestFactory
    {
        public static HtmlHelper<T> CreateHtmlHelper<T>(ViewDataDictionary viewData)
        {
            var builder = new TestControllerBuilder();
            var controllerContext = new ControllerContext(builder.HttpContext, builder.RouteData, new Mock<ControllerBase>().Object);
            var mockViewContext = new Mock<ViewContext>(controllerContext, new Mock<IView>().Object, viewData, new TempDataDictionary(), TextWriter.Null);
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData).Returns(viewData);
            return new HtmlHelper<T>(mockViewContext.Object, mockViewDataContainer.Object);
        }
    }
}
