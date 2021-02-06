using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Language.Tests
{
    public class InvalidParseTests
    {
        [Fact]
        public void ClosingParenthesesAfterCommand()
        {
            var result = Parser.ParseFromString("@Command)");

            Assert.NotNull(result);
            var nodes = result.Nodes;
            var list = nodes.First() as ListSyntax;
            var command = Assert.IsType<CommandSyntax>(list[0]);
            var text = Assert.IsType<TextSyntax>(list[1]);
            Assert.Equal("Command", command.Name.Value);
            Assert.Empty(command.Arguments);
            Assert.Equal(")", text.Text);
        }

        [Fact]
        public void MissingObjectField()
        {
            var result = Parser.ParseFromString("@Command({ field: A; }) rest");

            Assert.NotNull(result);
            var nodes = result.Nodes;
            var list = nodes.First() as ListSyntax;
            var command = Assert.IsType<CommandSyntax>(list[0]);
            var text = Assert.IsType<TextSyntax>(list[1]);
            Assert.Equal("Command", command.Name.Value);
            Assert.Single(command.Arguments);
            var obj = command.Arguments[0].Content[0] as ObjectSyntax;
            Assert.NotNull(obj);
            var field = Assert.Single(obj.Fields);
            Assert.Equal("field", field.Name.Value);
            Assert.Equal(" rest", text.Text);
        }

        [Fact]
        public void BracketsAfterCommand()
        {
            var result = Parser.ParseFromString("@Command[]");

            Assert.NotNull(result);
            var nodes = result.Nodes;
            var list = nodes.First() as ListSyntax;
            var command = Assert.IsType<CommandSyntax>(list[0]);
            var text = Assert.IsType<TextSyntax>(list[1]);
            Assert.Equal("Command", command.Name.Value);
            Assert.Empty(command.Arguments);
            Assert.Equal("[]", text.Text);
        }
    }
}
