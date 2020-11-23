using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Language.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("Some text", "Some text")]
        [InlineData("Some  text", "Some text")]
        [InlineData("Some\ttext", "Some text")]
        [InlineData("Some\ntext", "Some text")]
        [InlineData("Some \n text", "Some text")]
        public void TextParsingTest(string input, string output)
        {
            var text = ParseAsSyntax<TextSyntax>(input);
            Assert.Equal(output, text.Text);
        }

        [Theory]
        [InlineData("@Command", "Command")]
        [InlineData("@Command()", "Command")]
        [InlineData("@Command( )", "Command")]
        public void CommandWithoutArgumentsTest(string input, string commandName)
        {
            var command = ParseAsSyntax<CommandSyntax>(input);
            Assert.Equal(commandName, command.Name.Value);
            Assert.Equal(0, command.Arity);
            Assert.Empty(command.Arguments.Children);
        }

        [Theory]
        [InlineData("@Command()", 0)]
        [InlineData("@Command(x)", 1)]
        [InlineData("@Command(\"\")", 1)]
        [InlineData("@Command(Some text)", 1)]
        [InlineData("@Command(Some text; some more)", 2)]
        [InlineData("@Command(;)", 2)]
        public void CommandArgumentCountTest(string input, int arity)
        {
            var command = ParseAsSyntax<CommandSyntax>(input);
            Assert.Equal(arity, command.Arity);
        }

        [Fact]
        public void CommandArgumentParsingWithName()
        {
            var command = ParseAsSyntax<CommandSyntax>("@Command(argName: argValue)");
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

        [Fact]
        public void CommandObjectParsing()
        {
            var command = ParseAsSyntax<CommandSyntax>("@Command(obj: { fieldA: fieldValue; fieldB: fieldItem})");
            Assert.Equal(1, command.Arity);
            var argument = command.Arguments.Children.FirstOrDefault();
            Assert.NotNull(argument);
            Assert.NotNull(argument.Name);
            Assert.Equal("obj", argument.Name.Value);
            var argumentValue = argument.Content.Children.FirstOrDefault();
            var objSyntax = argumentValue.Children.FirstOrDefault() as ObjectSyntax;
            Assert.NotNull(objSyntax);
            Assert.Equal(2, objSyntax.Fields.Children.Count);
            var field1 = objSyntax.Fields.Children[0];
            var field2 = objSyntax.Fields.Children[1];
            Assert.Equal("fieldA", field1.Name.Value);
            Assert.Equal("fieldB", field2.Name.Value);
            var fieldValue = field1.Value.Children[0].Children[0] as TextSyntax;
            Assert.Equal("fieldValue", fieldValue.Text);
            var fieldItem = field2.Value.Children[0].Children[0] as TextSyntax;
            Assert.Equal("fieldItem", fieldItem.Text);
        }

        [Fact]
        public void ArrayAsArgument()
        {
            var result = ParseAsSyntax<CommandSyntax>("@Command([value1; value2])");
            var argument = result.Arguments.Children[0];
            var array = argument.Content.Children[0].Children[0] as ArraySyntax;
            var value1 = array.Children[0].Children[0].Children[0] as TextSyntax;
            var value2 = array.Children[1].Children[0].Children[0] as TextSyntax;
            Assert.Equal("value1", value1.Text);
            Assert.Equal("value2", value2.Text);
        }

        [Fact]
        public void ArrayAsText()
        {
            var result = ParseAsSyntax<TextSyntax>("[value1; value2]");
            Assert.Equal("[value1; value2]", result.Text);
        }

        private T ParseAsSyntax<T>(string input) where T : SyntaxNode
        {
            var result = Parser.ParseFromString(input).Nodes;
            var list = result.FirstOrDefault() as ListSyntax;
            Assert.NotNull(list);
            var element = list.Children.FirstOrDefault() as T;
            Assert.NotNull(element);
            return element;
        }
    }
}
