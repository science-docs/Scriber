using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public abstract class EngineInstruction
    {
        public Element Origin { get; set; }

        protected EngineInstruction(Element origin)
        {
            Origin = origin;
        }

        public abstract object? Execute(CompilerState state, object?[] arguments);

        private const string BeginCommand = "begin";
        private const string EndCommand = "end";

        public static EngineInstruction? Create(Element element)
        {
            if (element.Type == ElementType.Text || element.Type == ElementType.Quotation)
            {
                return new TextInstruction(element);
            }
            else if (element.Type == ElementType.Block || element.Type == ElementType.ExpliciteBlock)
            {
                return new BlockInstruction(element);
            }
            else if (element.Type == ElementType.Paragraph)
            {
                return new EmptyInstruction(element);
            }
            else if (element.Type == ElementType.Command)
            {
                // special cases for begin/end commands
                if (element.Content == BeginCommand)
                {
                    return new EnvironmentInstruction(element);
                }
                else if (element.Content == EndCommand)
                {
                    var envInstruction = new EnvironmentInstruction(element)
                    {
                        IsEnd = true
                    };
                    return envInstruction;
                }
                else
                {
                    var commandInstruction = new CommandInstruction(element);
                    return commandInstruction;
                }
            }
            else if (element.Type == ElementType.ObjectArray)
            {
                return new ObjectArrayInstruction(element);
            }
            else if (element.Type == ElementType.ObjectCreation)
            {
                return new ObjectCreationInstruction(element);
            }
            else if (element.Type == ElementType.ObjectField)
            {
                return new ObjectFieldInstruction(element);
            }
            else if (element.Type == ElementType.Null)
            {
                return new NullInstruction(element);
            }

            return null;
        }
    }
}
