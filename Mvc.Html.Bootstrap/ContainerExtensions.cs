using System.Collections.Generic;
using System.Web.Mvc;

namespace Mvc.Html.Bootstrap
{
    public static class ContainerExtensions
    {
        internal static MvcContainer ContainerHelper(this HtmlHelper html, string tagName)
        {
            return ContainerHelper(html, tagName, null);
        }

        internal static MvcContainer ContainerHelper(this HtmlHelper html, object htmlAttributes)
        {
            return ContainerHelper(html, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        internal static MvcContainer ContainerHelper(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            return ContainerHelper(html, "div", htmlAttributes);
        }

        internal static MvcContainer ContainerHelper(this HtmlHelper html, string tagName, IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(htmlAttributes);

            html.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new MvcContainer(html.ViewContext, tagName);
        }

        public static void EndContainer(this HtmlHelper html, string tagName)
        {
            EndContainer(html.ViewContext, tagName);
        }

        internal static void EndContainer(ViewContext viewContext, string tagName)
        {
            viewContext.Writer.Write("</{0}>", tagName);
        }
    }
}
