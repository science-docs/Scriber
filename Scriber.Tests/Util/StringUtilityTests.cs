using Xunit;

namespace Scriber.Util.Tests
{
    public class StringUtilityTests
    {
        [Theory]
        [InlineData("Word", true)]
        [InlineData("multiple words", false)]
        [InlineData("123", true)]
        [InlineData("number7", true)]
        public void IsWordTest(string input, bool result)
        {
            Assert.Equal(result, input.IsWord());
        }

        [Theory]
        [InlineData("Word", false)]
        [InlineData("123", true)]
        [InlineData("123word", false)]
        [InlineData("542.51", false)]
        public void IsNumberTest(string input, bool result)
        {
            Assert.Equal(result, input.IsNumber());
        }

        [Fact]
        public void SplitAtWhitespace()
        {
            var splits = "some value\tnext line ".Split(c => char.IsWhiteSpace(c));
            Assert.Equal(5, splits.Length);
            Assert.Equal("some", splits[0]);
            Assert.Equal("value", splits[1]);
            Assert.Equal("next", splits[2]);
            Assert.Equal("line", splits[3]);
            Assert.Equal("", splits[4]);
        }

        [Fact]
        public void SplitAtWhitespaceAndRemove()
        {
            var splits = "some  ".Split(c => char.IsWhiteSpace(c), System.StringSplitOptions.RemoveEmptyEntries);
            Assert.Single(splits);
            Assert.Equal("some", splits[0]);
        }

        [Theory]
        [InlineData("margin-bottom", "MarginBottom")]
        [InlineData("image", "Image")]
        [InlineData("", "")]
        [InlineData("MarginBottom", "MarginBottom")]
        public void TestToPascalCase(string input, string output)
        {
            Assert.Equal(output, input.ToPascalCase());
        }

        [Theory]
        [InlineData("margin-bottom", "margin-bottom")]
        [InlineData("image", "image")]
        [InlineData("", "")]
        [InlineData("MarginBottom", "margin-bottom")]
        public void TestToKebapCase(string input, string output)
        {
            Assert.Equal(output, input.ToKebapCase());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("Some", "Some")]
        [InlineData("some", "Some")]
        public void ToFirstUpperTest(string input, string output)
        {
            Assert.Equal(output, input.ToFirstUpper());
        }
    }
}
