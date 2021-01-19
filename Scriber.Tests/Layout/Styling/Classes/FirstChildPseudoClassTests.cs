using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class FirstChildPseudoClassTests
    {
        public FirstChildPseudoClass FirstChildPseudo = new FirstChildPseudoClass();

        [Fact]
        public void TestFirstChildSingle()
        {
            var p = new Paragraph();
            var leaf = new TextLeaf();
            p.Leaves.Add(leaf);
            Assert.True(FirstChildPseudo.Matches(leaf));
        }

        [Fact]
        public void TestFirstChildMultiple()
        {
            var p = new Paragraph();
            var leaf1 = new TextLeaf();
            var leaf2 = new TextLeaf();
            p.Leaves.Add(leaf1);
            p.Leaves.Add(leaf2);
            Assert.True(FirstChildPseudo.Matches(leaf1));
            Assert.False(FirstChildPseudo.Matches(leaf2));
        }
    }
}
