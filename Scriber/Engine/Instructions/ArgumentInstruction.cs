using Scriber.Language.Syntax;
using System.Linq;

namespace Scriber.Engine.Instructions
{
    public class ArgumentInstruction : EngineInstruction<ArgumentSyntax>
    {
        public override object? Execute(CompilerState state, ArgumentSyntax node)
        {
            var value = (EngineInstruction.Execute(state, node.Content).Value as Argument[])!;
            var result = value.Select(e => e.Value).ToArray();
            return result;
        }
    }
}
