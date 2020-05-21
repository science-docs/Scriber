using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class ObjectArrayInstruction : EngineInstruction
    {
        public ObjectArrayInstruction(Element origin) : base(origin)
        {
        }

        public override object? Execute(CompilerState state, object?[] arguments)
        {
            return new ObjectArray(Origin, arguments);
        }
    }
}
