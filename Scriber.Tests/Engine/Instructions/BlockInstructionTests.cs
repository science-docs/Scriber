using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class BlockInstructionTests
    {
        private Element E => ElementFixtures.EmptyElement();
        private BlockInstruction Block => new BlockInstruction(E);
        private CompilerState CS => CompilerStateFixtures.ReflectionLoaded();

        [Fact]
        public void EmptyBlock()
        {
            var blockContent = Block.Execute(CS, Array.Empty<Argument>());
            Assert.IsType<Argument[]>(blockContent);
            Assert.Empty((Argument[])blockContent);
        }

        [Fact]
        public void GroupedLeafBlock()
        {
            var firstLeaf = new TextLeaf("first");
            var secondLeaf = new TextLeaf("second");
            var blockContent = Block.Execute(CS, new Argument[] {
                new Argument(E, firstLeaf),
                new Argument(E, secondLeaf)
            });
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Single(argResult);
            Assert.IsType<Paragraph>(argResult[0].Value);
            var paragraph = (Paragraph)argResult[0].Value;
            Assert.Equal(firstLeaf, paragraph.Leaves[0]);
            Assert.Equal(secondLeaf, paragraph.Leaves[1]);
        }

        [Fact]
        public void MixedBlock()
        {
            var firstLeaf = new TextLeaf("first");
            var paragraph = Paragraph.FromText("intermediate");
            var secondLeaf = new TextLeaf("second");
            var blockContent = Block.Execute(CS, new Argument[] { 
                new Argument(E, firstLeaf),
                new Argument(E, paragraph),
                new Argument(E, secondLeaf)
            });
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Equal(3, argResult.Length);
            Assert.All(argResult, arg => Assert.IsType<Paragraph>(arg.Value));
            var firstParagraph = (Paragraph)argResult[0].Value;
            var secondParagraph = (Paragraph)argResult[2].Value;
            Assert.Equal(firstLeaf, firstParagraph.Leaves[0]);
            Assert.Equal(paragraph, argResult[1].Value);
            Assert.Equal(secondLeaf, secondParagraph.Leaves[0]);
        }

        [Fact]
        public void BlockWithParagraphBreak()
        {
            var firstLeaf = new TextLeaf("first");
            var paragraphBreak = EmptyInstruction.Object;
            var secondLeaf = new TextLeaf("second");
            var blockContent = Block.Execute(CS, new Argument[] {
                new Argument(E, firstLeaf),
                new Argument(E, paragraphBreak),
                new Argument(E, secondLeaf)
            });
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Equal(2, argResult.Length);
            Assert.All(argResult, arg => Assert.IsType<Paragraph>(arg.Value));
            var firstParagraph = (Paragraph)argResult[0].Value;
            var secondParagraph = (Paragraph)argResult[1].Value;
            Assert.Equal(firstLeaf, firstParagraph.Leaves[0]);
            Assert.Equal(secondLeaf, secondParagraph.Leaves[0]);
        }

        [Fact]
        public void BlockWithNullElement()
        {
            var paragraphBreak = NullInstruction.NullObject;
            var blockContent = Block.Execute(CS, new Argument[] {
                new Argument(E, paragraphBreak),
            });
            Assert.IsType<Argument[]>(blockContent);
            var argResult = (Argument[])blockContent;
            Assert.Single(argResult);
            Assert.Null(argResult[0].Value);
        }
    }
}
