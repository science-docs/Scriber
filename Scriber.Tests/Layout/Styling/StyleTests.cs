using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Layout.Styling.Tests
{
    public class StyleTests
    {
        [Fact]
        public void TestLocalContainer()
        {
            var doc = new Scriber.Document();
            var text = new TextLeaf("text")
            {
                Document = doc,
            };
            text.Style.Set(StyleKeys.FontSize, Unit.FromPoint(20));
            var fontSize = text.Style.Get(StyleKeys.FontSize);
            Assert.Equal(20, fontSize.Point);
        }

        [Fact]
        public void TestTagContainer()
        {
            var doc = new Scriber.Document();
            var spanStyle = new StyleContainer(StyleOrigin.Author, "span");
            doc.Styles.Add(spanStyle);
            var text = new TextLeaf("text")
            {
                Document = doc,
                Tag = "span"
            };
            spanStyle.Set(StyleKeys.FontSize, Unit.FromPoint(20));
            var fontSize = text.Style.Get(StyleKeys.FontSize);
            Assert.Equal(20, fontSize.Point);
        }

        [Fact]
        public void TestClassContainer()
        {
            var doc = new Scriber.Document();
            var spanStyle = new StyleContainer(StyleOrigin.Author, ".class");
            doc.Styles.Add(spanStyle);
            var text = new TextLeaf("text")
            {
                Document = doc
            };
            text.Classes.Add("class");
            spanStyle.Set(StyleKeys.FontSize, Unit.FromPoint(20));
            var fontSize = text.Style.Get(StyleKeys.FontSize);
            Assert.Equal(20, fontSize.Point);
        }
    }
}
