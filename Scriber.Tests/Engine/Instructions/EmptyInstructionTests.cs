using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class EmptyInstructionTests
    {
        [Fact]
        public void ConstructorNullOriginException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new EmptyInstruction(null);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var instruction = new EmptyInstruction(ElementFixtures.EmptyElement());
            var result = instruction.Execute(null, null);
            Assert.Equal(EmptyInstruction.Object, result);
        }
    }
}
