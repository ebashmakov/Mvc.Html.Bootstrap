using System.Web.Mvc;

namespace Mvc.Html.Bootstrap
{
    public enum AlertType
    {
        Warning, Error, Success, Info
    }

    public static class AlertExtensions
    {
        public static string DefaultCloseSymbol = "×";

        public static string ToCssClass(this AlertType type)
        {
            switch (type)
            {
                case AlertType.Warning:
                    return "alert-warning";
                case AlertType.Error:
                    return "alert-error";
                case AlertType.Success:
                    return "alert-success";
                case AlertType.Info:
                    return "alert-info";
                default:
                    return "alert";
            }
        }

        public static MvcHtmlString Alert(this HtmlHelper helper, AlertType type, string text)
        {
            return helper.Alert(type, text, string.Empty, false);
        }

        public static MvcHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header)
        {
            return helper.Alert(type, text, header, false);
        }

        public static MvcHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header, bool close)
        {
            return Alert(helper, type, text, header, ButtonTag.Anchor);
        }

        public static MvcHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header, ButtonTag closeButton)
        {
            var builder = new TagBuilder("div");

            builder.AddCssClass(type.ToCssClass());
            builder.AddCssClass("alert");

            builder.InnerHtml += DismissButton(helper, closeButton);

            if (!string.IsNullOrEmpty(header))
            {
                var headerTag = new TagBuilder("h4");
                headerTag.SetInnerText(header);
                builder.InnerHtml = headerTag.ToString();
            }

            builder.InnerHtml += text;
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DismissButton(this HtmlHelper html, ButtonTag tag)
        {
            return html.Button(DefaultCloseSymbol, tag, new { @class = "close", data_dismiss = "alert" });
        }

        public static MvcHtmlString DismissButton(this HtmlHelper html, ButtonTag tag, string text)
        {
            return html.Button(text, tag, new { @class = "close", data_dismiss = "alert" });
        }
    }
}
