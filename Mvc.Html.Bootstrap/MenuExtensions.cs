using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Html.Models;

namespace Mvc.Html.Bootstrap
{
    public static class MenuExtensions
    {
        public static IHtmlString GetBootstrappCssClass(this HtmlHelper html, SiteMapNodeModel node) {
            var css = new List<string>();

            if (node.IsCurrentNode || node.HasCurrentNode()) { css.Add("active"); }
            if (node.Children.Any()) { css.Add("dropdown"); }

            return html.Raw(css.Any() ? String.Join(" ", css) : string.Empty);
        }

        public static bool HasCurrentNode(this SiteMapNodeModel node)
        {
            if (node.Children.Where(n => n.IsCurrentNode).Any())
                return true;

            foreach (var childNode in node.Children)
            {
                if (childNode.HasCurrentNode())
                    return true;
            }

            return false;
        }
    }
}
