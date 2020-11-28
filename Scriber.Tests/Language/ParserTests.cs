using Scriber.Language.Syntax;
using System.Linq;
using Xunit;

namespace Scriber.Language.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("Some text", "Some text")]
        [InlineData("Some\\@text", "Some@text")]
        [InlineData("Some\\text", "Some\\text")]
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
            Assert.Empty(command.Arguments);
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
            var arguments = command.Arguments;
            Assert.Equal(command, arguments.Parent);
            var argument = arguments.FirstOrDefault();
            Assert.NotNull(argument);
            Assert.Equal(arguments, argument.Parent);
            Assert.NotNull(argument.Name);
            Assert.Equal("argName", argument.Name.Value);
            Assert.Equal(argument, argument.Name.Parent);
            var argumentValue = argument.Content.FirstOrDefault();
            Assert.Equal(argument, argument.Content.Parent);
            var textContent = argumentValue as TextSyntax;
            Assert.NotNull(textContent);
            Assert.Equal("argValue", textContent.Text);
            Assert.Equal(argument.Content, textContent.Parent);
        }

        [Fact]
        public void CommandObjectParsing()
        {
            var command = ParseAsSyntax<CommandSyntax>("@Command(obj: { fieldA: fieldValue ; fieldB: fieldItem})");
            Assert.Equal(1, command.Arity);
            var argument = command.Arguments.FirstOrDefault();
            Assert.NotNull(argument);
            Assert.NotNull(argument.Name);
            Assert.Equal("obj", argument.Name.Value);
            var argumentValue = argument.Content.FirstOrDefault();
            var objSyntax = argumentValue as ObjectSyntax;
            Assert.NotNull(objSyntax);
            Assert.Equal(2, objSyntax.Fields.Count);
            var field1 = objSyntax.Fields[0];
            var field2 = objSyntax.Fields[1];
            Assert.Equal("fieldA", field1.Name.Value);
            Assert.Equal("fieldB", field2.Name.Value);
            var fieldValue = field1.Value[0] as TextSyntax;
            Assert.Equal("fieldValue", fieldValue.Text);
            var fieldItem = field2.Value[0] as TextSyntax;
            Assert.Equal("fieldItem", fieldItem.Text);
        }

        [Fact]
        public void CommandNestedObjectParsing()
        {
            var command = ParseAsSyntax<CommandSyntax>("@Command(obj: { top: { nested: value }})");
            Assert.Equal(1, command.Arity);
            var argument = command.Arguments.FirstOrDefault();
            Assert.NotNull(argument);
            Assert.NotNull(argument.Name);
            Assert.Equal("obj", argument.Name.Value);
            var argumentValue = argument.Content.FirstOrDefault();
            var objSyntax = argumentValue as ObjectSyntax;
            Assert.NotNull(objSyntax);
            Assert.Single(objSyntax.Fields);
            var topField = objSyntax.Fields[0];
            Assert.Equal("top", topField.Name.Value);
            var nestedObject = topField.Value[0] as ObjectSyntax;
            Assert.NotNull(nestedObject);
            Assert.NotEmpty(nestedObject.Fields);
            var nestedField = nestedObject.Fields[0];
            Assert.Equal("nested", nestedField.Name.Value);
            var fieldValue = nestedField.Value[0] as TextSyntax;
            Assert.Equal("value", fieldValue.Text);
        }

        [Fact]
        public void ArrayAsArgument()
        {
            var result = ParseAsSyntax<CommandSyntax>("@Command([value1; value2])");
            var argument = result.Arguments[0];
            var array = argument.Content[0] as ArraySyntax;
            Assert.Equal(2, array.Arity);
            var value1 = array[0][0] as TextSyntax;
            var value2 = array[1][0] as TextSyntax;
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
            var element = list.FirstOrDefault() as T;
            Assert.NotNull(element);
            return element;
        }
    }
}
