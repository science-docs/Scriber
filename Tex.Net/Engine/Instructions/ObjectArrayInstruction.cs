namespace Tex.Net.Engine.Instructions
{
    public class ObjectArrayInstruction : EngineInstruction
    {
        public override object? Execute(CompilerState state, object[] arguments)
        {
            return new ObjectArray(arguments);
        }
    }
}
