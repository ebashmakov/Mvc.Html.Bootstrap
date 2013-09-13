using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq;


namespace Mvc.Html.Bootstrap
{
    public enum FormType
    {
        Default, Search, Inline, Horizontal
    }

    public static class FormExtensions
    {
        public static string ToCssClass(this FormType type)
        {
            switch (type)
            {
                case FormType.Search:
                    return "form-search";
                case FormType.Inline:
                    return "form-inline";
                case FormType.Horizontal:
                    return "form-horizontal";
                default:
                    return string.Empty;
            }
        }

        public static MvcContainer BeginControlGroup(this HtmlHelper html)
        {
            return BeginControlGroup(html, null);
        }

        public static MvcContainer BeginControlGroup(this HtmlHelper html, object htmlAttributes)
        {
            var tagName = "div";
            var tagBuilder = new TagBuilder(tagName);
            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            tagBuilder.AddCssClass("control-group");
            html.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new MvcContainer(html.ViewContext, tagName);
        }

        public static MvcForm BeginForm(this HtmlHelper html, FormType type)
        {
            var actionName = (string)html.ViewContext.RouteData.Values["action"];
            var controllerName = (string)html.ViewContext.RouteData.Values["controller"];
            return html.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = type.ToCssClass() });
        }

        public static MvcHtmlString ControlGroupFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText = null, IDictionary<string, object> htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            var isCheckbox = (metadata.ModelType == typeof(bool) || metadata.ModelType == typeof(bool?));

            var controls = new TagBuilder("div");
            controls.AddCssClass("controls");
            controls.InnerHtml += Environment.NewLine;
            controls.InnerHtml += htmlHelper.EditorFor(expression, new { placeholder = resolvedLabelText }) + Environment.NewLine;
            if (isCheckbox)
                controls.InnerHtml += resolvedLabelText + Environment.NewLine;
            controls.InnerHtml += htmlHelper.ValidationMessageFor(expression) + Environment.NewLine;

            var controlGroup = new TagBuilder("div");
            controlGroup.MergeAttributes(htmlAttributes, replaceExisting: true);
            controlGroup.AddCssClass("control-group");
            controlGroup.InnerHtml += Environment.NewLine;
            if (!isCheckbox)
                controlGroup.InnerHtml += htmlHelper.LabelFor(expression, resolvedLabelText, new { @class = "control-label" }) + Environment.NewLine;
            controlGroup.InnerHtml += controls + Environment.NewLine;
            
            return new MvcHtmlString(controlGroup.ToString(TagRenderMode.Normal));
        }
    }
}
