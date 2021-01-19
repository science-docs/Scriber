using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class OnlyChildPseudoClassTests
    {
        public OnlyChildPseudoClass OnlyChildPseudo = new OnlyChildPseudoClass();

        [Fact]
        public void TestFirstChildSingle()
        {
            var p = new Paragraph();
            TextLeaf? leaf = new TextLeaf();
            p.Leaves.Add(leaf);
            Assert.True(OnlyChildPseudo.Matches(leaf));
        }

        [Fact]
        public void TestFirstChildMultiple()
        {
            var p = new Paragraph();
            var leaf1 = new TextLeaf();
            var leaf2 = new TextLeaf();
            p.Leaves.Add(leaf1);
            p.Leaves.Add(leaf2);
            Assert.False(OnlyChildPseudo.Matches(leaf1));
            Assert.False(OnlyChildPseudo.Matches(leaf2));
        }
    }
}
