using Scriber.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class NullInstructionTests
    {
        [Fact]
        public void ConstructorNullOriginException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new NullInstruction(null);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var instruction = new NullInstruction(ElementFixtures.EmptyElement());
            var result = instruction.Execute(null, null);
            Assert.Equal(NullInstruction.NullObject, result);
        }
    }
}
