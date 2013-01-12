using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace Mvc.Html.Bootstrap
{
    public static class MenuExtensions
    {
        public static IHtmlString GetBootstrappCssClass(this HtmlHelper html, MvcSiteMapProvider.Web.Html.Models.SiteMapNodeModel node) {
            var css = new List<string>();
            if (node.IsCurrentNode) { css.Add("active"); }
            if (node.Children.Any()) { css.Add("dropdown"); }

            return html.Raw(css.Any() ? "class=\"" + String.Join(" ", css) + "\"" : string.Empty);
        }
    }
}
