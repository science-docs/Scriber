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
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MergedElementConverter(null);
            });
        }

        [Fact]
        public void NullTypeAddException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Add(null, new StringConverter());
            });
        }

        [Fact]
        public void NullConverterAddException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Add(typeof(string), null);
            });
        }

        [Fact]
        public void NullValueException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Convert(null, typeof(string));
            });
        }

        [Fact]
        public void NullTypeException()
        {
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());

            Assert.Throws<ArgumentNullException>(() =>
            {
                mergedConverter.Convert(Paragraph.FromText("notNull"), null);
            });
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
