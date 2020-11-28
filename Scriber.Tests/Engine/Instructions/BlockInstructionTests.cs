using Scriber.Language;
using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using System.Linq;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class BlockInstructionTests
    {
        private BlockInstruction Block => new BlockInstruction();
        private CompilerState CS => CompilerStateFixtures.ReflectionLoaded();

        [Fact]
        public void EmptyBlock()
        {
            var blockContent = Block.Evaluate(CS, new ListSyntax());
            Assert.IsType<Argument[]>(blockContent);
            Assert.Empty((Argument[])blockContent);
        }

        [Fact]
        public void StateNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Block.Evaluate(null, new ListSyntax());
            });
            Assert.Equal("state", ex.ParamName);
        }

        [Fact]
        public void ArgumentsNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Block.Evaluate(CS, null);
            });
            Assert.Equal("list", ex.ParamName);
        }

        [Fact]
        public void GroupedLeafBlock()
        {
            var blockContent = ParseBlock("some text");
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Single(argResult);
            Assert.IsType<Paragraph>(argResult[0].Value);
            var paragraph = (Paragraph)argResult[0].Value;
            var leaf = paragraph.Leaves[0] as ITextLeaf;
            Assert.Equal("some text", leaf.Content);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        [InlineData(" \n\t ")]
        public void WhitespaceBlock(string input)
        {
            var blockContent = ParseBlock(input);
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Empty(argResult);
        }

        [Fact]
        public void MixedBlock()
        {
            var blockContent = ParseBlock("first @Heading(1; A) second");
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Equal(3, argResult.Length);
            Assert.All(argResult, arg => Assert.IsType<Paragraph>(arg.Value));
            var firstParagraph = (Paragraph)argResult[0].Value;
            var firstLeaf = firstParagraph.Leaves[0] as ITextLeaf;
            var secondParagraph = (Paragraph)argResult[2].Value;
            var secondLeaf = secondParagraph.Leaves[0] as ITextLeaf;
            Assert.Equal("first ", firstLeaf.Content);
            Assert.Equal(" second", secondLeaf.Content);
        }

        private object ParseBlock(string input)
        {
            var nodes = Parser.ParseFromString(input).Nodes;
            var node = nodes.First() as ListSyntax;
            var result = Block.Evaluate(CS, node);
            return result;
        }
    }
}
