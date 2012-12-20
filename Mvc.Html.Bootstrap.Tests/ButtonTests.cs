using System.Collections;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;

namespace Mvc.Html.Bootstrap.Tests
{
    [TestFixture]
    public class ButtonTests
    {
        private readonly HtmlHelper _html;

        public ButtonTests()
        {
            var viewContext = new Mock<ViewContext>();
            var viewDataContainer = new Mock<IViewDataContainer>();

            _html = new HtmlHelper(viewContext.Object, viewDataContainer.Object);
        }

        IEnumerable TestData
        {
            get
            {
                yield return
                    new TestCaseData("Action", ButtonType.Default, ButtonSize.Default, ButtonTag.Default, false, false)
                        .Returns("<button class=\"btn\" type=\"button\">Action</button>");
            }
        }

        [Test]
        [TestCaseSource("TestData")]
        public string ButtonTest(string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block, bool? disabled)
        {
            return _html.Button(text, type, size, tag, block, disabled).ToHtmlString();
        }

        [Test]
        [TestCase("Action", Result = "<input class=\"btn btn-primary\" type=\"submit\" value=\"Action\" />")]
        public string SubmitTest(string text)
        {
            return _html.SubmitButton(text).ToHtmlString();
        }

    }
}
