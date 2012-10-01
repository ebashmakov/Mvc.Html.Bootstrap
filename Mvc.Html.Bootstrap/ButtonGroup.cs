using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Mvc.Html.Bootstrap
{
    public enum Direction
    {
        Horizontal, Vertical
    }

    public static class ButtonGroup
    {
        public static class Css
        {
            public static readonly string Group;
            public static readonly string VerticalGroup;
            public static readonly string Toolbar;

            static Css()
            {
                Group = "btn-group";
                VerticalGroup = string.Format("{0} {1}-vertical", Group, Group);
                Toolbar = "btn-toolbar";
            }
        }

        public static MvcContainer BeginButtonGroup(this HtmlHelper html)
        {
            return BeginButtonGroup(html, Direction.Horizontal);
        }

        public static MvcContainer BeginButtonGroup(this HtmlHelper html, Direction direction)
        {
            var cssClass = direction == Direction.Vertical
                ? Css.VerticalGroup
                : Css.Group;
            return html.ContainerHelper(new { @class = cssClass });
        }

        public static MvcContainer BeginButtonToolbar(this HtmlHelper html)
        {
            return html.ContainerHelper(new { @class = Css.Toolbar });
        }
    }
}
