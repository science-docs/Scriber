using System;
using System.Linq;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class ObjectCreationInstruction : EngineInstruction
    {
        private readonly Element? typeElement;

        public ObjectCreationInstruction(Element origin) : base(origin)
        {
            var parent = origin.Parent ?? throw new InvalidOperationException("A object creation instruction cannot be a top level element");
            if (parent.Children.Count <= 2)
            {
                origin.Siblings(out var previous, out var next);
                if (previous != null)
                {
                    typeElement = previous;
                }
                else if (next != null)
                {
                    throw new CompilerException(origin);
                }
            }
            else
            {
                throw new CompilerException(origin, "The object instantiation instruction contains invalid elements.");
            }
        }

        public override object? Execute(CompilerState state, object?[] arguments)
        {
            var parentBlock = state.Blocks.Peek(1);
            var creator = new ObjectCreator(Origin, state);

            // if a type specification exists
            if (parentBlock.Objects.Count == 1 && typeElement != null)
            {
                parentBlock.Objects.Clear();
                creator.TypeElement = typeElement;
                creator.TypeName = typeElement.Content;
            }
            else if (parentBlock.Objects.Count > 0)
            {
                throw new Exception();
            }

            creator.Fields.AddRange(arguments.Cast<ObjectField>());
            return creator;
        }
    }
}
