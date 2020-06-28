using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class LeafConverterTests
    {
        private readonly LeafConverter converter = new LeafConverter();

        [Fact]
        public void TextLeafToParagraph()
        {
            var textLeaf = new TextLeaf("text");
            var paragraph = converter.Convert(textLeaf, typeof(Paragraph));
            Assert.IsType<Paragraph>(paragraph);
            var par = (paragraph as Paragraph)!;
            var paragraphLeaf = par.Leaves[0];

            Assert.IsType<TextLeaf>(paragraphLeaf);
            var text = (paragraphLeaf as TextLeaf)!;
            Assert.Equal("text", text.Content);
        }

        [Fact]
        public void WrongTargetType()
        {
            Assert.Throws<ConverterException>(() =>
            {
                converter.Convert(new TextLeaf("123"), typeof(int));
            });
        }

        [Fact]
        public void WrongSourceType()
        {
            Assert.Throws<ConverterException>(() =>
            {
                converter.Convert("123", typeof(int));
            });
        }
    }
}
