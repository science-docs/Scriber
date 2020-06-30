using Scriber.Layout.Document;
using System;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class MergedElementConverterTests
    {
        [Fact]
        public void NullConverterConstructionException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new MergedElementConverter(null);
            });
            Assert.Equal("first", ex.ParamName);
        }

        [Fact]
        public void NullTypeAddException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Add(null, new StringConverter());
            });
            Assert.Equal("converterSourceType", ex.ParamName);
        }

        [Fact]
        public void NullConverterAddException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Add(typeof(string), null);
            });
            Assert.Equal("converter", ex.ParamName);
        }

        [Fact]
        public void NullValueException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Convert(null, typeof(string));
            });
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void NullTypeException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Convert(Paragraph.FromText("notNull"), null);
            });
            Assert.Equal("targetType", ex.ParamName);
        }

        [Fact]
        public void ParagraphToInt()
        {
            var paragraph = Paragraph.FromText("123");
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());
            mergedConverter.Add(typeof(string), new StringConverter());

            var value = mergedConverter.Convert(paragraph, typeof(int));
            Assert.IsType<int>(value);
            var intValue = (int)value;
            Assert.Equal(123, intValue);
        }

        [Fact]
        public void SimpleStringToInt()
        {
            var mergedConverter = new MergedElementConverter(new StringConverter());
            var value = mergedConverter.Convert("123", typeof(int));
            Assert.IsType<int>(value);
            var intValue = (int)value;
            Assert.Equal(123, intValue);
        }
    }
}
