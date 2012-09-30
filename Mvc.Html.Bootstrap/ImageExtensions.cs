using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc.Html.Bootstrap
{
    public enum ImageStyle
    {
        Rounded, Circle, Polaroid
    }

    public static class ImageExtensions
    {
        public static string ToCssClass(this ImageStyle style)
        {
            switch (style)
            {
                case ImageStyle.Rounded:
                    return "img-rounded";
                case ImageStyle.Circle:
                    return "img-circle";
                case ImageStyle.Polaroid:
                    return "img-polaroid";
                default:
                    return string.Empty;
            }
        }

        public static MvcHtmlString Image(this HtmlHelper html, string url)
        {
            return Image(html, url, null, null, null, null);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string url, ImageStyle style)
        {
            return Image(html, url, style, null, null, null);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string url, ImageStyle style, string id, string alternateText)
        {
            return Image(html, url, style, id, alternateText, null);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string url, string id, string alternateText, object htmlAttributes)
        {
            return Image(html, url, null, id, alternateText, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string url, ImageStyle? style, string id, string alternateText, object htmlAttributes)
        {
            var builder = new TagBuilder("img");

            builder.GenerateId(id);
            if (style.HasValue) builder.AddCssClass(style.Value.ToCssClass());
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
