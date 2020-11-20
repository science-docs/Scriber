using System;
using Xunit;

namespace Scriber.Maths.Tests
{
    public class MathParserTests
    {
        [Theory]
        [InlineData("0", 0)]
        [InlineData("3.4", 3.4)]
        [InlineData("-4", -4.0)]
        [InlineData("+4", 4.0)]
        [InlineData("5--4", 9.0)]
        [InlineData("5 - +4", 1.0)]
        [InlineData("5 -+ 4", 1.0)]
        [InlineData("5 +- 4", 1.0)]
        [InlineData("5 * -4", -20.0)]
        [InlineData("5 * +4", 20.0)]
        [InlineData("3 * 4", 12.0)]
        [InlineData("1.5 + 3.5", 5.0)]
        [InlineData("12 / 4", 3.0)]
        [InlineData("12 - 4", 8.0)]
        [InlineData("3 * 2 + 4", 10.0)]
        [InlineData("3 * (2 + 4)", 18.0)]
        [InlineData("3 * 2 * 4", 24.0)]
        [InlineData("3 * (2 * 4)", 24.0)]
        public void TestEvaluation(string input, double result)
        {
            var output = MathParser.Evaluate(input).AsDouble();
            Assert.Equal(result, output, 3);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData(".")]
        [InlineData(",")]
        [InlineData("3,4")]
        [InlineData("()")]
        [InlineData("3 * () + 4")]
        [InlineData("3 +++ 4")]
        public void InvalidEvaluation(string input)
        {
            Assert.ThrowsAny<Exception>(() => MathParser.Evaluate(input));
        }

        [Theory]
        [InlineData("3.4", true)]
        [InlineData("3,4", false)]
        [InlineData("", false)]
        [InlineData("3 + 8", true)]
        public void TestTryParse(string input, bool result)
        {
            var returnValue = MathParser.TryEvaluate(input, out var mathResult);
            Assert.NotNull(mathResult);
            Assert.Equal(result, returnValue);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("3.4", "3.4")]
        [InlineData("2+3", "2 + 3")]
        [InlineData("2+-3", "2 + -3")]
        [InlineData("2+(3*2)", "2 + (3 * 2)")]
        public void TokenString(string input, string target)
        {
            var output = MathParser.Parse(input).ToString();
            Assert.Equal(target, output);
        }
    }
}
