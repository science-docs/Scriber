using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class EmptyInstruction : EngineInstruction
    {
        public EmptyInstruction(Element origin) : base(origin)
        {
        }

        public static object Object { get; } = new object();

        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            return Object;
        }
    }
}
