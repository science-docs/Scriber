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

        public static EngineInstruction? Create(Element element)
        {
            switch (element.Type)
            {
                case ElementType.Text:
                case ElementType.Quotation:
                    return new TextInstruction(element);
                case ElementType.Block:
                case ElementType.ExplicitBlock:
                    return new BlockInstruction(element);
                case ElementType.Paragraph:
                    return new EmptyInstruction(element);
                case ElementType.Command:
                    return new CommandInstruction(element);
                case ElementType.Environment:
                    return new EnvironmentInstruction(element);
                case ElementType.ObjectArray:
                    return new ObjectArrayInstruction(element);
                case ElementType.ObjectCreation:
                    return new ObjectCreationInstruction(element);
                case ElementType.ObjectField:
                    return new ObjectFieldInstruction(element);
                case ElementType.Null:
                    return new NullInstruction(element);
            }

            return null;
        }
    }
}
