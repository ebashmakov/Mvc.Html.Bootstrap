using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq;
//using System.ComponentModel.DataAnnotations.Schema;

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
            var htmlHelper = MvcTestHelper.GetHtmlHelper<object>(viewData, out writer);

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

        [Test]
        public void ControlGroupFor()
        {
            using (MockViewEngine engine = new MockViewEngine())
            {
                // Arrange
                var htmlHelper = MvcTestHelper.GetHtmlHelper<LoginModel>(GetControlGroupForViewData());
                ViewContext callbackViewContext = null;
                engine.Engine.Setup(e => e.FindPartialView(htmlHelper.ViewContext, "DisplayTemplates/String", true))
                    .Returns(new ViewEngineResult(engine.View.Object, engine.Engine.Object))
                    .Verifiable();
                engine.View.Setup(v => v.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                    .Callback<ViewContext, TextWriter>((vc, tw) =>
                    {
                        callbackViewContext = vc;
                        tw.Write("View Text");
                    })
                    .Verifiable();
                // Act
                var actual = htmlHelper.ControlGroupFor(m => m.UserName).ToString();
                // Assert
                var expected = "<div class=\"control-group\">" + Environment.NewLine
                    + "<label class=\"control-label\" for=\"UserName\">User name</label>" + Environment.NewLine
                    + "<div class=\"controls\">" + Environment.NewLine
                    //+ "<input data-val=\"true\" data-val-required=\"The User name field is required.\" id=\"UserName\" name=\"UserName\" placeholder=\"User name\" type=\"text\" value=\"\" />" + Environment.NewLine
                    + "View Text" + Environment.NewLine
                    + "<span class=\"field-validation-valid\" data-valmsg-for=\"UserName\" data-valmsg-replace=\"true\"></span>" + Environment.NewLine
                    + "</div>" + Environment.NewLine
                    + "</div>" + Environment.NewLine;
                
                //engine.Engine.Verify();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        private static ViewDataDictionary<LoginModel> GetControlGroupForViewData()
        {
            var viewData = new ViewDataDictionary<LoginModel>();
            return viewData;
        }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class MockViewEngine : IDisposable
    {
        List<IViewEngine> oldEngines;

        public MockViewEngine(bool returnView = true)
        {
            oldEngines = ViewEngines.Engines.ToList();

            View = new Mock<IView>();

            Engine = new Mock<IViewEngine>();

            Engine.Setup(e => e.FindPartialView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(returnView ? new ViewEngineResult(View.Object, Engine.Object) : new ViewEngineResult(new string[0]));

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(Engine.Object);
        }

        public void Dispose()
        {
            ViewEngines.Engines.Clear();

            foreach (IViewEngine engine in oldEngines)
            {
                ViewEngines.Engines.Add(engine);
            }
        }

        public Mock<IViewEngine> Engine { get; set; }

        public Mock<IView> View { get; set; }
    }
}
