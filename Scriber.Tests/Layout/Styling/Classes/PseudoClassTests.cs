using System;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class PseudoClassTests
    {
        [Theory]
        [InlineData("root", typeof(RootPseudoClass))]
        [InlineData("not(p)", typeof(NotPseudoClass))]
        [InlineData("nth-child(4n+3)", typeof(NthChildPseudoClass))]
        [InlineData("first-child", typeof(FirstChildPseudoClass))]
        [InlineData("only-child", typeof(OnlyChildPseudoClass))]
        [InlineData("last-child", typeof(LastChildPseudoClass))]
        public void ParseTestByType(string input, Type targetType)
        {
            var parsedPseudoClass = PseudoClass.FromString(input);
            Assert.IsType(targetType, parsedPseudoClass);
        }
    }
}
