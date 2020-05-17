using System;
using System.Linq;
using Scriber.Layout.Document;

namespace Scriber.Engine.Instructions
{
    public class ObjectCreationInstruction : EngineInstruction
    {
        public override object? Execute(CompilerState state, object[] arguments)
        {
            var parentBlock = state.Blocks.Peek(1);
            var creator = new ObjectCreator();

            // if a type specification exists
            if (parentBlock.Objects.Count == 1 && parentBlock.Objects[0] is TextLeaf text)
            {
                parentBlock.Objects.Clear();
                creator.Type = text.Content;
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
