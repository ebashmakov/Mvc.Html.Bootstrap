using System.Web.Mvc;

namespace Mvc.Html.Bootstrap
{
    public enum TableStyle
    {
        Default, Striped, Bordered, Hover, Condensed
    }

    public enum RawStyle
    {
        Default, Success, Error, Warning, Info
    }

    public static class TableExtensions
    {
        public static string ToCssClass(this TableStyle style)
        {
            switch (style)
            {
                case TableStyle.Striped:
                    return "table-striped";
                case TableStyle.Bordered:
                    return "table-bordered";
                case TableStyle.Hover:
                    return "table-hover";
                case TableStyle.Condensed:
                    return "table-condensed";
                default:
                    return "table";
            }
        }

        public static string ToTableCssClass(this TableStyle style)
        {
            return string.Format("{0} {1}", TableStyle.Default.ToCssClass(), style.ToCssClass());
        }

        public static string ToCssClass(this RawStyle style)
        {
            switch (style)
            {
                case RawStyle.Success:
                    return "success";
                case RawStyle.Error:
                    return "error";
                case RawStyle.Warning:
                    return "warning";
                case RawStyle.Info:
                    return "info";
                default:
                    return string.Empty;
            }
        }

        public static MvcContainer BeginTable(this HtmlHelper html)
        {
            return BeginTable(html, TableStyle.Default);
        }

        public static MvcContainer BeginTable(this HtmlHelper html, TableStyle style)
        {
            return html.ContainerHelper(new { @class = style.ToTableCssClass() });
        }
    }
}
