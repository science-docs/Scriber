using Scriber.Language;
using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Language.Tests
{
    public class CommentParserTest
    {
        [Fact]
        public void TestWithSpace()
        {
            var element = ParseAsSyntax("/*test*/ end");
            Assert.IsType<CommentSyntax>(element[0]);
            Assert.IsType<TextSyntax>(element[1]);
        }

        [Fact]
        public void TestWithoutSpaceAfter()
        {
            var element = ParseAsSyntax("/*test*/end");
            Assert.IsType<CommentSyntax>(element[0]);
            Assert.IsType<TextSyntax>(element[1]);
        }

        [Fact]
        public void TestWithoutSpaceBefore()
        {
            var element = ParseAsSyntax("start/*test*/");
            Assert.IsType<TextSyntax>(element[0]);
            Assert.IsType<CommentSyntax>(element[1]);
        }

        [Fact]
        public void TestIncorrectStart()
        {
            var element = ParseAsSyntax("/test*/ end");
            Assert.Single(element);
            Assert.IsType<TextSyntax>(element[0]);
        }

        private SyntaxNode[] ParseAsSyntax(string input)
        {
            var result = Parser.ParseFromString(input).Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            return list.ToArray();
        }
    }
}
