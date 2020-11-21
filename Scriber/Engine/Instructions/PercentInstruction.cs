using Scriber.Language;
using Scriber.Variables;
using System.Globalization;

namespace Scriber.Engine.Instructions
{
    class PercentInstruction : EngineInstruction
    {
        public PercentInstruction(Element origin) : base(origin)
        {
        }

        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            var width = state.Document.Variable(PageVariables.BoxSize).Width.ToString(CultureInfo.InvariantCulture);
            return $"*{width}pt";
        }
    }
}
