using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class ParagraphConverterTests
    {
        private readonly ParagraphConverter converter = new ParagraphConverter();

        [Fact]
        public void NullValueException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                converter.Convert(null, typeof(string));
            });
        }

        [Fact]
        public void NullTypeException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                converter.Convert(Paragraph.FromText("notNull"), null);
            });
        }

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
            Assert.Equal("text", first.Content);
        }

        [Fact]
        public void WrongTargetType()
        {
            var paragraph = Paragraph.FromText("text");
            Assert.Throws<ConverterException>(() =>
            {
                converter.Convert(paragraph, typeof(int));
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
