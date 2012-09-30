using System.Web.Mvc;

namespace Mvc.Html.Bootstrap
{
    public enum ButtonType
    {
        Defualt, Primary, Info, Success, Warning, Danger, Inverse, Link
    }

    public enum ButtonSize
    {
        Defualt, Large, Small, Mini
    }

    public enum ButtonTag
    {
        Defualt, Anchor, Input, InputSubmit
    }

    public static class ButtonExtensions
    {
        public static string ToCssClass(this ButtonType type)
        {
            switch (type)
            {
                case ButtonType.Primary:
                    return "btn-primary";
                case ButtonType.Info:
                    return "btn-success";
                case ButtonType.Success:
                    return "btn-success";
                case ButtonType.Warning:
                    return "btn-warning";
                case ButtonType.Danger:
                    return "btn-danger";
                case ButtonType.Inverse:
                    return "btn-inverse";
                case ButtonType.Link:
                    return "btn-link";
                default:
                    return "btn";
            }
        }

        public static string ToCssClass(this ButtonSize type)
        {
            switch (type)
            {
                case ButtonSize.Large:
                    return "btn-large";
                case ButtonSize.Small:
                    return "btn-small";
                case ButtonSize.Mini:
                    return "btn-mini";
                default:
                    return string.Empty;
            }
        }

        public static string ToHtmlTag(this ButtonTag type)
        {
            switch (type)
            {
                case ButtonTag.Anchor:
                    return "a";
                case ButtonTag.Input:
                    return "input";
                case ButtonTag.InputSubmit:
                    return "input";
                default:
                    return "button";
            }
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text)
        {
            return Button(html, text, ButtonType.Defualt);
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text, ButtonType type)
        {
            return Button(html, text, type, ButtonSize.Defualt);
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text, ButtonType type, ButtonSize size)
        {
            return Button(html, text, type, size, ButtonTag.Defualt);
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag)
        {
            return Button(html, text, type, size, tag, false, false);
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block)
        {
            return Button(html, text, type, size, tag, block, false);
        }

        public static MvcHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block, bool? disabled)
        {
            var tagName = (tag.HasValue ? tag.Value : ButtonTag.Defualt).ToHtmlTag();
            var builder = new TagBuilder(tagName);

            // Adding css styles
            if (disabled.HasValue && disabled.Value)
            {
                builder.AddCssClass("disabled");
                builder.MergeAttribute("disabled", "disabled");
            }
            if (block.HasValue && block.Value) builder.AddCssClass("btn-block");
            if (size.HasValue) builder.AddCssClass(size.Value.ToCssClass());
            if (type.HasValue) builder.AddCssClass(type.Value.ToCssClass());
            builder.AddCssClass(ButtonType.Defualt.ToCssClass());

            // Adding html tag type
            if (tag.HasValue)
            {
                if (tag == ButtonTag.Defualt || tag == ButtonTag.Input)
                {
                    builder.MergeAttribute("type", "button");
                }
                if (tag == ButtonTag.InputSubmit)
                {
                    builder.MergeAttribute("type", "submit");
                }
            }

            builder.SetInnerText(text);
            return new MvcHtmlString(builder.ToString());
        }
    }
}
