using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public abstract class EngineInstruction
    {
        public abstract object? Execute(CompilerState state, object[] arguments);

        private const string BeginCommand = "begin";
        private const string EndCommand = "end";

        public static EngineInstruction? Create(Element element)
        {
            if (element.Type == ElementType.Text)
            {
                return TextInstruction.Create(element);
            }
            else if (element.Type == ElementType.Block)
            {
                return new BlockInstruction();
            }
            else if (element.Type == ElementType.Command)
            {
                // special cases for begin/end commands
                if (element.Content == BeginCommand)
                {
                    return EnvironmentInstruction.Create();
                }
                else if (element.Content == EndCommand)
                {
                    var envInstruction = EnvironmentInstruction.Create();
                    envInstruction.IsEnd = true;
                    return envInstruction;
                }
                else
                {
                    var commandInstruction = CommandInstruction.Create(element);
                    return commandInstruction;
                }
            }
            return null;
        }
    }
}
