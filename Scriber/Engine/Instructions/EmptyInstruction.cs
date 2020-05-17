namespace Scriber.Engine.Instructions
{
    public class EmptyInstruction : EngineInstruction
    {
        public static object Object { get; } = new object();

        public override object? Execute(CompilerState state, object[] arguments)
        {
            return Object;
        }
    }
}
