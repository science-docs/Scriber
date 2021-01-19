using Scriber.Language.Syntax;
using Scriber.Variables;
using System.Globalization;

namespace Scriber.Engine.Instructions
{
    public class PercentInstruction : EngineInstruction<PercentSyntax>
    {
        public override object? Evaluate(CompilerState state, PercentSyntax node)
        {
            var width = state.Document.Variable(PageVariables.BoxSize).Width;
            width /= 100.0;
            var widthText = width.ToString(CultureInfo.InvariantCulture);
            return $"*{widthText}pt";
        }
    }
}
