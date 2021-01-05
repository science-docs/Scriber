using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class NotPseudoClassTests
    {
        [Fact]
        public void NotParagraph()
        {
            var pSelector = new TagStyleSelector("p");
            var not = new NotPseudoClass(pSelector);
            var p = new Paragraph();
            Assert.True(pSelector.Matches(p));
            Assert.False(not.Matches(p));
        }

        [Fact]
        public void NotSpan()
        {
            var pSelector = new TagStyleSelector("p");
            var not = new NotPseudoClass(pSelector);
            var leaf = new TextLeaf
            {
                Tag = "span"
            };
            Assert.False(pSelector.Matches(leaf));
            Assert.True(not.Matches(leaf));
        }
    }
}
