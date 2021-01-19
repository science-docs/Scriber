using Scriber.Layout;
using Scriber.Tests.Fixture;
using Scriber.Variables;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class PercentInstructionTest
    {
        private readonly PercentInstruction instruction = new PercentInstruction();

        [Fact]
        public void ReturnsValueTest()
        {
            var cs = CompilerStateFixtures.Empty();
            PageVariables.Size.Set(cs.Document, new Size(410, 0));
            PageVariables.Margin.Set(cs.Document, Thickness.Zero);
            var result = instruction.Evaluate(cs, new Language.Syntax.PercentSyntax());
            Assert.IsType<string>(result);
            Assert.Equal("*4.1pt", result as string);
        }
    }
}
