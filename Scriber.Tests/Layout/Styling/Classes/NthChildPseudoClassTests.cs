using Scriber.Layout.Document;
using System;
using System.Linq;
using Xunit;

namespace Scriber.Layout.Styling.Classes.Tests
{
    public class NthChildPseudoClassTests
    {
        [Theory]
        [InlineData("odd", 2, 1, 1, 3, 5, 7, 9, 11)]
        [InlineData("even", 2, 0, 2, 4, 6, 8)]
        [InlineData("ODD", 2, 1)]
        [InlineData("EVEN", 2, 0)]
        [InlineData("4", 0, 4, 4)]
        [InlineData("4n", 4, 0, 4, 8, 12)]
        [InlineData("4N", 4, 0)]
        [InlineData("4n + 3", 4, 3, 3, 7, 11, 15)]
        [InlineData("2n+5", 2, 5, 5, 7, 9, 11)]
        public void ParseCorrectly(string input, int step, int offset, params int[] indices)
        {
            var pseudoClass = new NthChildPseudoClass(input);
            Assert.Equal(step, pseudoClass.Step);
            Assert.Equal(offset, pseudoClass.Offset);

            var paragraph = new Paragraph();
            int max = indices.Length == 0 ? 0 : indices.Max();
            Leaf[] leaves = new Leaf[max];
            for (int i = 0; i < max; i++)
            {
                leaves[i] = new TextLeaf();
            }
            paragraph.Leaves.AddRange(leaves);

            for (int i = 0; i < max; i++)
            {
                Assert.Equal(indices.Contains(i + 1), pseudoClass.Matches(leaves[i]));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("4n + 3n")]
        [InlineData("3 + 4n")]
        [InlineData("three n plus one")]
        public void ParseError(string input)
        {
            Assert.Throws<ArgumentException>(() => new NthChildPseudoClass(input));
        }

        [Fact]
        public void ParseNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new NthChildPseudoClass(null));
        }
    }
}
