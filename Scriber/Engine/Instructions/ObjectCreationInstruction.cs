using System;
using System.Linq;
using Scriber.Language;

namespace Scriber.Engine.Instructions
{
    public class ObjectCreationInstruction : EngineInstruction
    {
        public Element? TypeElement { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="CompilerException"/>
        public ObjectCreationInstruction(Element origin) : base(origin)
        {
            var parent = origin.Parent ?? throw new ArgumentException("A object creation instruction cannot be a top level element", nameof(origin));
            if (parent.Children.Count <= 2)
            {
                origin.Siblings(out var previous, out var next);
                if (previous != null)
                {
                    TypeElement = previous;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object? Execute(CompilerState state, Argument[] arguments)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var parentBlock = state.Blocks.Peek(1);
            var creator = new ObjectCreator(Origin, state);

            // if a type specification exists
            if (parentBlock.Objects.Count == 1 && TypeElement != null)
            {
                parentBlock.Objects.Clear();
                creator.TypeElement = TypeElement;
                creator.TypeName = TypeElement.Content;
            }
            else if (parentBlock.Objects.Count > 0)
            {
                throw new InvalidOperationException();
            }

            creator.Fields.AddRange(arguments.Select(e => e.Value).Cast<ObjectField>());
            return creator;
        }
    }
}
