using Scriber.Layout.Document;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class ParagraphConverterTests
    {
        private readonly ParagraphConverter converter = new ParagraphConverter();

        [Fact]
        public void ParagraphToString()
        {
            var paragraph = Paragraph.FromText("text");
            var converted = converter.Convert(paragraph, typeof(string));
            Assert.IsType<string>(converted);
            Assert.Equal("text", converted);
        }

        [Fact]
        public void ParagraphToLeaves()
        {
            var paragraph = Paragraph.FromText("text");
            var converted = converter.Convert(paragraph, typeof(IEnumerable<Leaf>));
            Assert.IsAssignableFrom<IEnumerable<Leaf>>(converted);
            var leaves = (IEnumerable<Leaf>)converted;
            Assert.Single(leaves);
            var first = leaves.First() as ITextLeaf;
            Assert.NotNull(first);
            Assert.Equal("text", first!.Content);
        }
    }
}
