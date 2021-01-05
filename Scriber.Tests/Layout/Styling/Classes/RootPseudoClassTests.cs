using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class RootPseudoClassTests
    {
        public RootPseudoClass RootPseudo = new RootPseudoClass();

        [Fact]
        public void TestDocument()
        {
            var doc = new Scriber.Document();
            var p = new Paragraph();
            doc.Elements.Add(p);
            Assert.False(RootPseudo.Matches(p));
            Assert.True(RootPseudo.Matches(doc));
        }
    }
}
