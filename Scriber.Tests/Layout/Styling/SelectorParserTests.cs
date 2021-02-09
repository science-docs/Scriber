using System;
using Xunit;

namespace Scriber.Layout.Styling.Tests
{
    public class SelectorParserTests
    {
        [Theory]
        [InlineData("p")]
        [InlineData("h1")]
        [InlineData("some-tag")]
        [InlineData("    whitespace-tag\t \n")]
        public void TagParsing(string text)
        {
            var selector = StyleSelectorParser.Parse(text);
            Assert.IsType<TagStyleSelector>(selector);
        }

        [Theory]
        [InlineData(".some")]
        [InlineData(".some-class")]
        [InlineData(".one.two")]
        [InlineData("  .whitespace  ")]
        public void ClassParsing(string text)
        {
            var selector = StyleSelectorParser.Parse(text);
            Assert.IsType<ClassStyleSelector>(selector);
        }

        [Fact]
        public void AllParsing()
        {
            var selector = StyleSelectorParser.Parse("*");
            Assert.IsType<AllStyleSelector>(selector);
        }

        [Theory]
        [InlineData(":root > p", typeof(PseudoClassSelector), typeof(TagStyleSelector))]
        [InlineData(":root .class p", typeof(PseudoClassSelector), typeof(ClassStyleSelector), typeof(TagStyleSelector))]
        [InlineData("a+.b", typeof(TagStyleSelector), typeof(ClassStyleSelector))]
        public void ComplexTypeParsing(string text, params Type[] types)
        {
            var selector = StyleSelectorParser.Parse(text);
            var complex = Assert.IsType<ComplexStyleSelector>(selector);
            int i = 0;
            while (complex.Right is ComplexStyleSelector right)
            {
                Assert.IsType(types[i++], complex.Left);
                complex = right;
            }
            Assert.Equal(types.Length - 2, i);
            Assert.IsType(types[^2], complex.Left);
            Assert.IsType(types[^1], complex.Right);
        }

        [Theory]
        [InlineData(":root > p", StyleOperator.Child)]
        [InlineData(":root .class~p", StyleOperator.Descendant, StyleOperator.Previous)]
        [InlineData("a+b", StyleOperator.Next)]
        [InlineData("p:nth-child(even)", StyleOperator.And)]
        public void ComplexOperatorParsing(string text, params StyleOperator[] operators)
        {
            var selector = StyleSelectorParser.Parse(text);
            var complex = Assert.IsType<ComplexStyleSelector>(selector);
            int i = 0;
            while (complex.Right is ComplexStyleSelector right)
            {
                Assert.Equal(operators[i++], complex.Operator);
                complex = right;
            }
            Assert.Equal(operators.Length - 1, i);
            Assert.Equal(operators[^1], complex.Operator);
        }
    }
}
