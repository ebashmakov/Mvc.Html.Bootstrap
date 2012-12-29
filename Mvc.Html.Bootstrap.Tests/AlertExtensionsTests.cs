using System;
using System.Web.Mvc;
using System.Xml;
using Mvc.Html.Bootstrap;
using NUnit.Framework;

namespace Mvc.Html.Bootstrap.Tests
{
    [TestFixture]
    public class AlertExtensionsTests
    {
        [Test]
        public void DismissButtonDefault()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper();

            // Act
            var html = helper.DismissButton(ButtonTag.Default).ToHtmlString();

            // Assert
            Assert.AreEqual(
                "<button class=\"close\" data_dismiss=\"alert\" type=\"button\">&#215;</button>",
                html);
        }

        [Test]
        public void DismissButtonAnchor()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper();

            // Act
            var html = helper.DismissButton(ButtonTag.Anchor).ToHtmlString();

            // Assert
            Assert.AreEqual(
                "<a class=\"close\" data_dismiss=\"alert\" href=\"#\">&#215;</a>",
                html);
        }

        [Test]
        public void DismissButtonInput()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper();

            // Act
            var html = helper.DismissButton(ButtonTag.Input).ToHtmlString();

            // Assert
            Assert.AreEqual(
                "<input class=\"close\" data_dismiss=\"alert\" type=\"button\">&#215;</input>",
                html);
        }

        [Test]
        public void DismissButtonInputSubmit()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper();

            // Act
            var html = helper.DismissButton(ButtonTag.InputSubmit).ToHtmlString();

            // Assert
            Assert.AreEqual(
                "<input class=\"close\" data_dismiss=\"alert\" type=\"submit\" value=\"×\" />",
                html);
        }

        [Test]
        public void DismissButtonCustomText()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper();

            // Act
            var html = helper.DismissButton(ButtonTag.Default, "<Foo>").ToHtmlString();

            // Assert
            Assert.AreEqual(
                "<button class=\"close\" data_dismiss=\"alert\" type=\"button\">&lt;Foo&gt;</button>",
                html);
        }
    }
}
