using Tex.Net.Language;

namespace Tex.Net.Engine.Instructions
{
    public abstract class EngineInstruction
    {
        public abstract object? Execute(CompilerState state, object[] arguments);

        private const string BeginCommand = "begin";
        private const string EndCommand = "end";

        public static EngineInstruction? Create(Element element)
        {
            if (element.Type == ElementType.Text || element.Type == ElementType.Quotation)
            {
                return TextInstruction.Create(element);
            }
            else if (element.Type == ElementType.Block || element.Type == ElementType.ExpliciteBlock)
            {
                return new BlockInstruction();
            }
            else if (element.Type == ElementType.Paragraph)
            {
                return new EmptyInstruction();
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
            else if (element.Type == ElementType.ObjectArray)
            {
                return new ObjectArrayInstruction();
            }
            else if (element.Type == ElementType.ObjectCreation)
            {
                return new ObjectCreationInstruction();
            }
            else if (element.Type == ElementType.ObjectField)
            {
                return new ObjectFieldInstruction(element);
            }

            return null;
        }
    }
}
