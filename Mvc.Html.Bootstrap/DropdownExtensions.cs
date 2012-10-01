using System.Web.Mvc;

namespace Mvc.Html.Bootstrap
{
    public static class DropdownExtensions
    {
        public static class Css
        {
            public static readonly string Dropdown;
            public static readonly string DropdownMenu;
            public static readonly string DropdownSubmenu;

            static Css()
            {
                Dropdown = "dropdown";
                DropdownMenu = Dropdown + "-menu";
                DropdownSubmenu = Dropdown + "-submenu";
            }
        }

        public static MvcContainer BeginDropdown(this HtmlHelper html)
        {
            var @class = Css.Dropdown;
            return html.ContainerHelper(new { @class });
        }

        public static MvcContainer BeginDropdownMenu(this HtmlHelper html)
        {
            var @class = Css.DropdownMenu;
            var role = "menu";
            var aria_labelledby = "dropdownMenu";
            return html.ContainerHelper(new { @class, role, aria_labelledby });
        }

        public static MvcContainer BeginDropdownMenu(this HtmlHelper html, string aria)
        {
            var @class = Css.DropdownMenu;
            var role = "menu";
            var aria_labelledby = aria;
            return html.ContainerHelper(new { @class, role, aria_labelledby });
        }

        public static MvcContainer BeginDropdownSubmenu(this HtmlHelper html)
        {
            var @class = Css.DropdownSubmenu;
            return html.ContainerHelper(new { @class });
        }

        public static MvcHtmlString MenuItem(this HtmlHelper html, string text)
        {
            return MenuItem(html, text, "#");
        }

        public static MvcHtmlString MenuItem(this HtmlHelper html, string text, string url)
        {
            var builder = new TagBuilder("li");

            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("tabindex", "-1");
            anchor.MergeAttribute("href", url);
            anchor.SetInnerText(text);

            builder.InnerHtml += anchor;
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString MenuSeparator(this HtmlHelper html)
        {
            var builder = new TagBuilder("li");
            builder.AddCssClass("divider");
            return new MvcHtmlString(builder.ToString());
        }
    }
}
