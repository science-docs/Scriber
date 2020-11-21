using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Language.Tests
{
    public class ParserTests
    {
        [Fact]
        public void TextParsingTest()
        {
            var result = Parser.ParseFromString("Some text").Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            var text = list.Children.FirstOrDefault() as TextSyntax;
            Assert.NotNull(text);
            Assert.Equal("Some text", text.Text);
        }

        [Theory]
        [InlineData("@Command", "Command")]
        [InlineData("@Command()", "Command")]
        [InlineData("@Command( )", "Command")]
        public void CommandWithoutArgumentsTest(string input, string commandName)
        {
            var result = Parser.ParseFromString(input).Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            var command = list.Children.FirstOrDefault() as CommandSyntax;
            Assert.NotNull(command);
            Assert.Equal(commandName, command.Name.Value);
            Assert.Equal(0, command.Arity);
            Assert.Empty(command.Arguments.Children);
        }

        [Fact]
        public void CommandArgumentParsingWithName()
        {
            var result = Parser.ParseFromString("@Command(argName: argValue)").Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            var command = list.Children.FirstOrDefault() as CommandSyntax;
            Assert.NotNull(command);
            Assert.Equal(1, command.Arity);
            var argument = command.Arguments.Children.FirstOrDefault();
            Assert.NotNull(argument);
            Assert.NotNull(argument.Name);
            Assert.Equal("argName", argument.Name.Value);
            var argumentValue = argument.Content.Children.FirstOrDefault();
            var textContent = argumentValue.Children.FirstOrDefault() as TextSyntax;
            Assert.NotNull(textContent);
            Assert.Equal("argValue", textContent.Text);
        }
    }
}
