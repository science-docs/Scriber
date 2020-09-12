using Xunit;

namespace Scriber.Util.Tests
{
    public class TexUtilityTests
    {
        [Theory]
        [InlineData('o', '`', 'ò')]
        [InlineData('o', '\'', 'ó')]
        [InlineData('o', '^', 'ô')]
        [InlineData('o', '"', 'ö')]
        [InlineData('o', 'H', 'ő')]
        [InlineData('o', '~', 'õ')]
        [InlineData('c', 'c', 'ç')]
        [InlineData('a', 'k', 'ą')]
        [InlineData('o', '=', 'ō')]
        [InlineData('o', 'b', 'o')]
        [InlineData('o', '.', 'ȯ')]
        [InlineData('u', 'd', 'ụ')]
        [InlineData('a', 'r', 'å')]
        [InlineData('o', 'u', 'ŏ')]
        [InlineData('s', 'v', 'š')]
        [InlineData('a', ' ', 'a')]
        public void SuccessfulComposition(char original, char diacritic, char result)
        {
            var resultChar = TexUtility.CompositeCharacter(original, diacritic);
            Assert.Equal(result, resultChar);
        }
    }
}
