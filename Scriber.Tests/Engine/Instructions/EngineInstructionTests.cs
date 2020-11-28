using Scriber.Language;
using Scriber.Language.Syntax;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class EngineInstructionTests
    {
        [Fact]
        public void NullElementException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                EngineInstruction.Evaluate(null, new ListSyntax());
            });
            Assert.Equal("state", ex.ParamName);
        }

        [Fact]
        public void NonValidTypeException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                EngineInstruction.Evaluate(CompilerStateFixtures.Empty(), null);
            });
            Assert.Equal("node", ex.ParamName);
        }
    }
}
