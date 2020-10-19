using Xunit;

namespace Scriber.Tests
{
    public class PageNumberingModuleTests
    {
        private PageNumberingModule Module => new PageNumberingModule();

        [Theory]
        [InlineData(4, PageNumberingStyle.Arabic, "4")]
        [InlineData(27, PageNumberingStyle.Arabic, "27")]
        [InlineData(800_000, PageNumberingStyle.Arabic, "800000")]
        [InlineData(4, PageNumberingStyle.Alphabet, "d")]
        [InlineData(27, PageNumberingStyle.Alphabet, "aa")]
        [InlineData(800_000, PageNumberingStyle.Alphabet, "asmkf")]
        [InlineData(27, PageNumberingStyle.AlphabetUpper, "AA")]
        [InlineData(4, PageNumberingStyle.Roman, "iv")]
        [InlineData(4, PageNumberingStyle.RomanUpper, "IV")]
        public void ValueTest(int value, PageNumberingStyle style, string result)
        {
            var module = Module;
            module.Style = style;
            module.Current = value;
            var x = module.Next();
            Assert.Equal(result, x);
        }

        [Fact]
        public void NextTest()
        {
            var module = Module;
            module.Current = 4;
            module.Style = PageNumberingStyle.Arabic;
            Assert.Equal("4", module.Next());
            Assert.Equal("5", module.Next());
        }

        [Fact]
        public void LessThanOne()
        {
            var module = Module;
            module.Current = -50;
            Assert.Equal(1, module.Current);
        }

        [Fact]
        public void ResetSuccess()
        {
            var module = Module;
            module.Current = 15;
            module.Style = PageNumberingStyle.Roman;
            module.Reset();
            Assert.Equal(1, module.Current);
            Assert.Equal(PageNumberingStyle.Arabic, module.Style);
        }
    }
}
