using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class TextInstructionTests
    {
        private static readonly TextInstruction textInstruction = new TextInstruction();

        [Fact]
        public void ExecuteNullStateException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                textInstruction.Evaluate(null, new TextSyntax());
            });
            Assert.Equal("state", ex.ParamName);
        }

        [Fact]
        public void ExecuteNullSyntaxNodeException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                textInstruction.Evaluate(CompilerStateFixtures.Empty(), null);
            });
            Assert.Equal("text", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var result = textInstruction.Evaluate(CompilerStateFixtures.Empty(), new TextSyntax
            {
                Text = "text"
            });
            Assert.IsType<TextLeaf>(result);
            var text = (TextLeaf)result;
            Assert.Equal("text", text.Content);
        }
    }
}
