using MvcSiteMapProvider.Web.Html.Models;
using NUnit.Framework;

namespace Mvc.Html.Bootstrap.Tests
{
    [TestFixture]
    public class MenuExtensionsTests
    {
        [Test]
        public void HasCurrentNodeWithNoChildernReturnsFalse()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            // Act
            // Assert
            Assert.That(root.HasCurrentNode(), Is.False);
        }

        [Test]
        public void HasCurrentNodeWithFirstLevelChildCurrentReturnsTrue()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            root.Children.Add(new SiteMapNodeModel { IsCurrentNode = true });
            // Act
            // Assert
            Assert.That(root.HasCurrentNode(), Is.True);
        }

        [Test]
        public void HasCurrentNodeWithSecondLevelChildCurrentReturnsTrue()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            var child1 = new SiteMapNodeModel();
            child1.Children.Add(new SiteMapNodeModel { IsCurrentNode = true });
            root.Children.Add(child1);
            // Act
            // Assert
            Assert.That(root.HasCurrentNode(), Is.True);
        }

        [Test]
        public void HasCurrentNodeWithNoCurrentChildrenReturnsFalse()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            var child1 = new SiteMapNodeModel();
            child1.Children.Add(new SiteMapNodeModel());
            root.Children.Add(child1);
            // Act
            // Assert
            Assert.That(root.HasCurrentNode(), Is.False);
        }

        [Test]
        public void GetBootstrapCssClassWithChildrenNoneAreCurrent()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            var child1 = new SiteMapNodeModel();
            child1.Children.Add(new SiteMapNodeModel());
            root.Children.Add(child1);
            // Act
            var htmlHelper = MvcTestHelper.GetHtmlHelper();
            var actual = htmlHelper.GetBootstrappCssClass(root).ToHtmlString();
            // Assert
            Assert.That(actual, Is.EqualTo("dropdown"));
        }

        [Test]
        public void GetBootstrapCssClassWithSecondLevelChildCurrent()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            var child1 = new SiteMapNodeModel();
            child1.Children.Add(new SiteMapNodeModel { IsCurrentNode = true });
            root.Children.Add(child1);
            // Act
            var htmlHelper = MvcTestHelper.GetHtmlHelper();
            var actual = htmlHelper.GetBootstrappCssClass(root).ToHtmlString();
            // Assert
            Assert.That(actual, Is.EqualTo("active dropdown"));
        }

        [Test]
        public void GetBootstrapCssClassWithRootNodeCurrent()
        {
            // Arrange
            var root = new SiteMapNodeModel { IsCurrentNode = true };
            // Act
            var htmlHelper = MvcTestHelper.GetHtmlHelper();
            var actual = htmlHelper.GetBootstrappCssClass(root).ToHtmlString();
            // Assert
            Assert.That(actual, Is.EqualTo("active"));
        }

        [Test]
        public void GetBootstrapCssClassWithRootNodeNotCurrent()
        {
            // Arrange
            var root = new SiteMapNodeModel();
            // Act
            var htmlHelper = MvcTestHelper.GetHtmlHelper();
            var actual = htmlHelper.GetBootstrappCssClass(root).ToHtmlString();
            // Assert
            Assert.That(actual, Is.Empty);
        }
    }
}
