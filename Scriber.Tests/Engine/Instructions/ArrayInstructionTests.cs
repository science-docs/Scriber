using Scriber.Language.Syntax;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class ArrayInstructionTests
    {
        private static readonly ArrayInstruction arrayInstruction = new ArrayInstruction();

        [Fact]
        public void EvaluateStateNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                arrayInstruction.Evaluate(null, new ArraySyntax());
            });
            Assert.Equal("state", ex.ParamName);
        }

        [Fact]
        public void EvaluateArrayNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                arrayInstruction.Evaluate(CompilerStateFixtures.Empty(), null);
            });
            Assert.Equal("array", ex.ParamName);
        }

        [Fact]
        public void EvaluateEmpty()
        {
            var node = new ArraySyntax();
            var result = arrayInstruction.Evaluate(CompilerStateFixtures.Empty(), node);
            Assert.IsType<ObjectArray>(result);
            var array = (ObjectArray)result;
            Assert.Equal(node, array.Origin);
            Assert.Empty(array.Get(typeof(object)));
        }
    }
}
