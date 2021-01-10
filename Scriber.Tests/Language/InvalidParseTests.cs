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
            var command = list[0] as CommandSyntax;
            var text = list[1] as TextSyntax;
            Assert.IsType<CommandSyntax>(command);
            Assert.IsType<TextSyntax>(text);
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
            var command = list[0] as CommandSyntax;
            var text = list[1] as TextSyntax;
            Assert.IsType<CommandSyntax>(command);
            Assert.IsType<TextSyntax>(text);
            Assert.Equal("Command", command.Name.Value);
            Assert.Single(command.Arguments);
            var obj = command.Arguments[0].Content[0] as ObjectSyntax;
            Assert.NotNull(obj);
            Assert.Single(obj.Fields);
            Assert.Equal("field", obj.Fields[0].Name.Value);
            Assert.Equal(" rest", text.Text);
        }

        [Fact]
        public void BracketsAfterCommand()
        {
            var result = Parser.ParseFromString("@Command[]");

            Assert.NotNull(result);
            var nodes = result.Nodes;
            var list = nodes.First() as ListSyntax;
            var command = list[0] as CommandSyntax;
            var text = list[1] as TextSyntax;
            Assert.IsType<CommandSyntax>(command);
            Assert.IsType<TextSyntax>(text);
            Assert.Equal("Command", command.Name.Value);
            Assert.Empty(command.Arguments);
            Assert.Equal("[]", text.Text);
        }
    }
}
