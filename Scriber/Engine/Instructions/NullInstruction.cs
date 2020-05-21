using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class NullInstruction : EngineInstruction
    {
        public static object NullObject { get; } = new object();

        public NullInstruction(Element origin) : base(origin)
        {
        }

        public override object? Execute(CompilerState state, object?[] arguments)
        {
            return NullObject;
        }
    }
}
