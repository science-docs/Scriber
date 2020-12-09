using Scriber.Language.Syntax;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Scriber.Language.Tests
{
    public class InvalidParseTests
    {
        [Fact]
        public void ClosingParenthesesAfterCommand()
        {
            ParserResult? result = null;
            Assert.True(Terminates(() =>
            {
                result = Parser.ParseFromString("@Command)");
            }));

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
            ParserResult? result = null;
            Assert.True(Terminates(() =>
            {
                result = Parser.ParseFromString("@Command({ field: A; }) rest");
            }));

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
            ParserResult? result = null;
            Assert.True(Terminates(() =>
            {
                result = Parser.ParseFromString("@Command[]");
            }));

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

        public static bool Terminates(Action action)
        {
            var task = Task.Run(action);
            var stopwatch = Stopwatch.StartNew();

            while (task.Status != TaskStatus.RanToCompletion)
            {
                if (stopwatch.ElapsedMilliseconds > 100)
                {
                    stopwatch.Stop();
                    return false;
                }
            }
            stopwatch.Stop();
            return true;
        }
    }
}
