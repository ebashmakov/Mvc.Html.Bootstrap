using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Xml;

namespace Mvc.Html.Bootstrap
{
    public static class ValidationExtensions
    {
        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper htmlHelper, string message)
        {
            return BootstrapValidationSummary(htmlHelper, message, AlertType.Error);
        }

        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper htmlHelper, string message, AlertType alertType)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("helper");
            }

            if (htmlHelper.ViewData.ModelState.IsValid)
            {
                // No client side validation
                return null;
            }

            var xml = new XmlDocument();
            xml.LoadXml(htmlHelper.ValidationSummary(message).ToString());
            // Add the dismiss button
            var buttonFrag = xml.CreateDocumentFragment();
            buttonFrag.InnerXml = htmlHelper.DismissButton(ButtonTag.Default).ToHtmlString();
            xml.DocumentElement.InsertBefore(buttonFrag, xml.DocumentElement.FirstChild);
            // Rename the message <span> to <h4>
            var messageNode = xml.SelectSingleNode("//div/span");
            var newMessageNode = xml.CreateElement("h4");
            newMessageNode.InnerXml = messageNode.InnerXml;
            xml.DocumentElement.InsertBefore(newMessageNode, messageNode);
            xml.DocumentElement.RemoveChild(messageNode);
            // Add bootstrap CSS classes
            var cssClass = xml.DocumentElement.GetAttribute("class");
            xml.DocumentElement.SetAttribute("class", cssClass + " alert alert-block " + alertType.ToCssClass());

            return MvcHtmlString.Create(xml.OuterXml);
        }
    }
}
