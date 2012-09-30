using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Mvc.Html.Bootstrap
{
    public enum FormType
    {
        Default, Search, Inline, Horizontal
    }

    public static class FormExtensions
    {
        public static string ToCssClass(this FormType type)
        {
            switch (type)
            {
                case FormType.Search:
                    return "form-search";
                case FormType.Inline:
                    return "form-inline";
                case FormType.Horizontal:
                    return "form-horizontal";
                default:
                    return string.Empty;
            }
        }

        public static MvcContainer BeginControlGroup(this HtmlHelper html)
        {
            return html.ContainerHelper(new { @class = "control-group" });
        }

        public static MvcForm BeginForm(this HtmlHelper html, FormType type)
        {
            var actionName = (string)html.ViewContext.RouteData.Values["action"];
            var controllerName = (string)html.ViewContext.RouteData.Values["controller"];
            return html.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = type.ToCssClass() });
        }
    }
}
