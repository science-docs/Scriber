using Scriber.Layout.Document;
using System;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class ConverterCollectionTests
    {
        private readonly ConverterCollection converters = new ConverterCollection();

        [Fact]
        public void AddConverterNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.Add(null, typeof(string));
            });
            Assert.Equal("converter", ex.ParamName);
        }

        [Fact]
        public void AddSourceTypeNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.Add(new StringConverter(), null);
            });
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void AddTargetTypeNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.Add(new StringConverter(), typeof(string), null);
            });
            Assert.Equal("targets", ex.ParamName);
        }

        [Fact]
        public void AddTargetTypeNullElementException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                converters.Add(new StringConverter(), typeof(string), new Type[] { null });
            });
            Assert.Equal("targets", ex.ParamName);
        }

        [Fact]
        public void FindSourceTypeNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.Find(null, typeof(int));
            });
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void FindTargetTypeNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.Find(typeof(string), null);
            });
            Assert.Equal("target", ex.ParamName);
        }

        [Fact]
        public void TryConvertSourceNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.TryConvert(null, typeof(int), out var _);
            });
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void TryConvertTypeNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converters.TryConvert(new object(), null, out var _);
            });
            Assert.Equal("target", ex.ParamName);
        }

        [Fact]
        public void AddFindConverter()
        {
            converters.Add(new StringConverter(), typeof(string), typeof(int));
            var found = converters.Find(typeof(string), typeof(int));
            Assert.NotNull(found);
            Assert.IsType<StringConverter>(found);
        }

        [Fact]
        public void TryConvertString()
        {
            converters.Add(new StringConverter(), typeof(string), typeof(int));
            Assert.True(converters.TryConvert("123", typeof(int), out var value));
            var intValue = Assert.IsType<int>(value);
            Assert.Equal(123, intValue);
        }

        [Fact]
        public void TryConvertFalse()
        {
            converters.Add(new StringConverter(), typeof(string), typeof(int));
            Assert.False(converters.TryConvert("123", typeof(long), out var _));
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("10000000000")]
        public void TryConvertException(string input)
        {
            converters.Add(new StringConverter(), typeof(string), typeof(int));
            Assert.ThrowsAny<ConverterException>(() =>
            {
                converters.TryConvert(input, typeof(int), out var _);
            });
        }

        [Fact]
        public void ResolvePathsWorks()
        {
            converters.Add(new StringConverter(), typeof(string), typeof(int));
            converters.Add(new ParagraphConverter(), typeof(Paragraph), typeof(string));
            converters.ResolvePaths();

            var resolvedConverter = converters.Find(typeof(Paragraph), typeof(int));
            Assert.NotNull(resolvedConverter);
            var paragraph = Paragraph.FromText("123");
            var value = resolvedConverter.Convert(paragraph, typeof(int));
            Assert.Equal(123, value);
        }
    }
}
