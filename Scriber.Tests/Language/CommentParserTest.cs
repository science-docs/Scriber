using Scriber.Language;
using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Tests.Language
{
    public class CommentParserTest
    {
        [Fact]
        public void TestWithoutSpace()
        {
            var element = ParseAsSyntax<CommentSyntax>("/*test*/ end");

        }


        private T ParseAsSyntax<T>(string input) where T : SyntaxNode
        {
            var result = Parser.ParseFromString(input).Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            var element = list.FirstOrDefault() as T;
            Assert.NotNull(element);
            return element;
        }
    }
}
