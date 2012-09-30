using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Mvc.Html.Bootstrap
{
    public static class ContainerExtensions
    {
        internal static MvcForm ContainerHelper(this HtmlHelper html, object htmlAttributes)
        {
            return ContainerHelper(html, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        internal static MvcForm ContainerHelper(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(htmlAttributes);

            html.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new MvcForm(html.ViewContext); ;
        }

        public static void EndContainer(this HtmlHelper html)
        {
            EndContainer(html.ViewContext);
        }

        internal static void EndContainer(ViewContext viewContext)
        {
            viewContext.Writer.Write("</div>");
        }
    }
}
