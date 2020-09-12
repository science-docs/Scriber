using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class ObjectArrayInstructionTests
    {
        [Fact]
        public void ConstructorNullOriginException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectArrayInstruction(null);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ExecuteStateNullException()
        {
            var instruction = new ObjectArrayInstruction(ElementFixtures.EmptyElement());
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                instruction.Execute(null, Array.Empty<Argument>());
            });
            Assert.Equal("state", ex.ParamName);
        }

        [Fact]
        public void ExecuteArgumentsNullException()
        {
            var instruction = new ObjectArrayInstruction(ElementFixtures.EmptyElement());
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                instruction.Execute(CompilerStateFixtures.ReflectionLoaded(), null);
            });
            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var instruction = new ObjectArrayInstruction(ElementFixtures.EmptyElement());
            var result = instruction.Execute(CompilerStateFixtures.ReflectionLoaded(), Array.Empty<Argument>());
            Assert.IsType<ObjectArray>(result);
        }
    }
}
