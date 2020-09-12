using Xunit;

namespace Scriber.Util.Tests
{
    public class StringUtilityTests
    {
        [Theory]
        [InlineData(" d", "d", 1)]
        [InlineData(" d ", "d ", 1)]
        [InlineData("d", "d", 0)]
        [InlineData(" ", "", 1)]
        public void AdvanceTrimStart(string value, string target, int advance)
        {
            var result = value.TrimStart(out var ad);
            if (result.Length > 0)
            {
                Assert.False(char.IsWhiteSpace(result[0]));
                if (char.IsWhiteSpace(value[^1]))
                {
                    Assert.True(char.IsWhiteSpace(result[^1]));
                }
            }
            Assert.Equal(target, result);
            Assert.Equal(advance, ad);
        }

        [Theory]
        [InlineData(" d ", "d", 1)]
        public void AdvanceTrim(string value, string target, int advance)
        {
            var result = value.Trim(out var ad);
            if (result.Length > 0)
            {
                Assert.False(char.IsWhiteSpace(result[0]));
                Assert.False(char.IsWhiteSpace(result[^1]));
            }
            Assert.Equal(target, result);
            Assert.Equal(advance, ad);
        }
    }
}
