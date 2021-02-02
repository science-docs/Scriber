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
            var spanStyle = new StyleContainer("span");
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
            var spanStyle = new StyleContainer(".class");
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

        [Fact]
        public void TestParentContainer()
        {
            var doc = new Scriber.Document();
            Assert.True(StyleKeys.FontSize.Inherited);
            doc.Style.Set(StyleKeys.FontSize, Unit.FromPoint(20));
            var text = new TextLeaf("text")
            {
                Document = doc
            };
            text.Parent = doc;
            var fontSize = text.Style.Get(StyleKeys.FontSize);
            Assert.Equal(20, fontSize.Point);
        }

        [Fact]
        public void TestDefaultStyle()
        {
            Assert.Equal(Unit.FromPoint(12), StyleKeys.FontSize.Default);
            var text = new TextLeaf("text")
            {
                Document = new Scriber.Document()
            };
            var fontSize = text.Style.Get(StyleKeys.FontSize);
            Assert.Equal(12, fontSize.Point);
        }
    }
}
