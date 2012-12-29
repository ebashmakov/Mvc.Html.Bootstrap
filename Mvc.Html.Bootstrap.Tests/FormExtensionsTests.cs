using System;
using System.Web.Mvc;
using System.Xml;
using Mvc.Html.Bootstrap;
using NUnit.Framework;
using System.IO;
using System.Web.Mvc.Html;
using System.Web.Routing;


namespace Mvc.Html.Bootstrap.Tests
{
    [TestFixture]
    public class FormExtensionsTests
    {
        private static void BeginMvcContainerHelper(Func<HtmlHelper, MvcContainer> beginContainer, string expectedContainerTag)
        {
            // Arrange
            var viewData = new ViewDataDictionary();
            StringWriter writer;
            //var htmlHelper = HtmlHelperTestFactory.CreateHtmlHelper<string>(viewData, out writer);
            var htmlHelper = MvcTestHelper.GetHtmlHelper(viewData, out writer);

            // Act
            IDisposable containerDisposable = beginContainer(htmlHelper);
            containerDisposable.Dispose();

            // Assert
            Assert.AreEqual(expectedContainerTag + "</div>", writer.ToString());
        }

        private static void BeginMvcFormHelper(Func<HtmlHelper, MvcForm> beginForm, string expectedFormTag)
        {
            // Arrange
            var viewData = new ViewDataDictionary();
            StringWriter writer;
            var htmlHelper = MvcTestHelper.GetFormHelper(out writer);

            // Act
            IDisposable formDisposable = beginForm(htmlHelper);
            formDisposable.Dispose();

            // Assert
            Assert.AreEqual(expectedFormTag + "</form>", writer.ToString());
        }

        [Test]
        public void BeginControlGroup()
        {
            BeginMvcContainerHelper(html => html.BeginControlGroup(), "<div class=\"control-group\">");
        }

        [Test]
        public void BeginControlGroupWithHtmlAttributes()
        {
            BeginMvcContainerHelper(html => html.BeginControlGroup(new { @class = "test-cssclass", id = "foo" }), "<div class=\"control-group test-cssclass\" id=\"foo\">");
        }

        [Test]
        public void BeginHorizontalForm()
        {
            BeginMvcFormHelper(html => html.BeginForm(FormType.Horizontal), @"<form action=""" + MvcTestHelper.AppPathModifier + @"/home/index"" class=""form-horizontal"" method=""post"">");
        }

        [Test]
        public void BeginInlineForm()
        {
            BeginMvcFormHelper(html => html.BeginForm(FormType.Inline), @"<form action=""" + MvcTestHelper.AppPathModifier + @"/home/index"" class=""form-inline"" method=""post"">");
        }

        [Test]
        public void BeginSearchForm()
        {
            BeginMvcFormHelper(html => html.BeginForm(FormType.Search), @"<form action=""" + MvcTestHelper.AppPathModifier + @"/home/index"" class=""form-search"" method=""post"">");
        }

        [Test]
        public void BeginDefaultForm()
        {
            BeginMvcFormHelper(html => html.BeginForm(FormType.Default), @"<form action=""" + MvcTestHelper.AppPathModifier + @"/home/index"" class="""" method=""post"">");
        }
    }
}
