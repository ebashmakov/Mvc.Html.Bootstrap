using System;
using System.Web.Mvc;
using System.Xml;
using Mvc.Html.Bootstrap;
using NUnit.Framework;

namespace Mvc.Html.Bootstrap.Tests
{
    [TestFixture]
    public class ValidationExtensionsTests
    {
        [Test]
        public void BootstrapValidationSummary()
        {
            // Arrange
            var helper = MvcTestHelper.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            var html = helper.BootstrapValidationSummary("Please fix the following errors.");

            // Assert
            Assert.AreEqual("<div class=\"validation-summary-errors alert alert-block alert-error\">"
                + "<button class=\"close\" data_dismiss=\"alert\" type=\"button\">×</button>"
                + "<h4>Please fix the following errors.</h4>"
                + "<ul><li>foo error &lt;1&gt;</li>"
                + "<li>foo error 2</li>"
                + "<li>bar error &lt;1&gt;</li>"
                + "<li>bar error 2</li>"
                + "</ul></div>",
                html.ToHtmlString());
        }


        private static ViewDataDictionary<ValidationModel> GetViewDataWithModelErrors()
        {
            ViewDataDictionary<ValidationModel> viewData = new ViewDataDictionary<ValidationModel>();
            ModelState modelStateFoo = new ModelState();
            ModelState modelStateBar = new ModelState();
            ModelState modelStateBaz = new ModelState();

            modelStateFoo.Errors.Add(new ModelError(new InvalidOperationException("foo error from exception")));
            modelStateFoo.Errors.Add(new ModelError("foo error <1>"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            modelStateBar.Errors.Add(new ModelError("bar error <1>"));
            modelStateBar.Errors.Add(new ModelError("bar error 2"));

            viewData.ModelState["foo"] = modelStateFoo;
            viewData.ModelState["bar"] = modelStateBar;
            viewData.ModelState["baz"] = modelStateBaz;

            viewData.ModelState.SetModelValue("quux", new ValueProviderResult(null, "quuxValue", null));
            viewData.ModelState.AddModelError("quux", new InvalidOperationException("Some error text."));
            return viewData;
        }
    }


    public class ValidationModel
    {
        public string foo { get; set; }
        public string bar { get; set; }
        public string baz { get; set; }
        public string quux { get; set; }
    }
}
