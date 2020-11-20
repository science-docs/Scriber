using Scriber.Language;
using System;

namespace Scriber.Engine.Instructions
{
    public abstract class EngineInstruction : Traceable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        protected EngineInstruction(Element origin) : base(origin)
        {
        }

        public abstract object? Execute(CompilerState state, Argument[] arguments);

        public static EngineInstruction? Create(Element element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

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
                case ElementType.Environment:
                case ElementType.Command:
                    return new CommandInstruction(element);
                case ElementType.ObjectArray:
                    return new ObjectArrayInstruction(element);
                case ElementType.ObjectCreation:
                    return new ObjectCreationInstruction(element);
                case ElementType.ObjectField:
                    return new ObjectFieldInstruction(element);
                case ElementType.Null:
                    return new NullInstruction(element);
                case ElementType.Comment:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), $"Unknown element type '{element.Type}'.");
            }
        }
    }
}
