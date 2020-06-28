using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class ConverterCollectionTests
    {
        private readonly ConverterCollection converters = new ConverterCollection();

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
            Assert.IsType<int>(value);
            Assert.Equal(123, value);
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
            var value = resolvedConverter!.Convert(paragraph, typeof(int));
            Assert.Equal(123, value);
        }
    }
}
