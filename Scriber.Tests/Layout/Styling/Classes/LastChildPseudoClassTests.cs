using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class LastChildPseudoClassTests
    {
        public LastChildPseudoClass LastChildPseudo = new LastChildPseudoClass();

        [Fact]
        public void TestLastChildSingle()
        {
            var p = new Paragraph();
            var leaf = new TextLeaf();
            p.Leaves.Add(leaf);
            Assert.True(LastChildPseudo.Matches(leaf));
        }

        [Fact]
        public void TestLastChildMultiple()
        {
            var p = new Paragraph();
            var leaf1 = new TextLeaf();
            var leaf2 = new TextLeaf();
            p.Leaves.Add(leaf1);
            p.Leaves.Add(leaf2);
            Assert.False(LastChildPseudo.Matches(leaf1));
            Assert.True(LastChildPseudo.Matches(leaf2));
        }
    }
}
