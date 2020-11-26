using Scriber.Language.Syntax;
using Scriber.Variables;
using System.Globalization;

namespace Scriber.Engine.Instructions
{
    public class PercentInstruction : EngineInstruction<PercentSyntax>
    {
        public override object? Execute(CompilerState state, PercentSyntax node)
        {
            var width = state.Document.Variable(PageVariables.BoxSize).Width.ToString(CultureInfo.InvariantCulture);
            return $"*{width}pt";
        }
    }
}
