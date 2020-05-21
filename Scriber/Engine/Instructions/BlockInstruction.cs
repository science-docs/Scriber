using System.Collections.Generic;
using Scriber.Language;
using Scriber.Layout.Document;

namespace Scriber.Engine.Instructions
{
    public class BlockInstruction : EngineInstruction
    {
        public BlockInstruction(Element origin) : base(origin)
        {
        }

        public override object Execute(CompilerState state, object?[] arguments)
        {
            var results = new List<object?>();

            // Group continueous leafs into one paragraph
            Paragraph? currentParagraph = null;

            for (int i = 0; i < arguments.Length; i++)
            {
                var item = arguments[i];
                if (item is Leaf leaf)
                {
                    if (currentParagraph == null)
                    {
                        currentParagraph = new Paragraph();
                    }

                    currentParagraph.Leaves.Add(leaf);
                }
                else if (item == EmptyInstruction.Object)
                {
                    ResetParagraph(results, ref currentParagraph);
                }
                else if (item == NullInstruction.NullObject)
                {
                    ResetParagraph(results, ref currentParagraph);
                    results.Add(null);
                }
                else
                {
                    ResetParagraph(results, ref currentParagraph);
                    results.Add(item);
                }
            }

            if (currentParagraph != null)
            {
                results.Add(currentParagraph);
            }

            if (results.Count == 0 && Origin.Type == ElementType.ExplicitBlock)
            {
                // signaling an empty block
                results.Add(null);
            }

            return results.ToArray();
        }

        private static void ResetParagraph(List<object?> results, ref Paragraph? paragraph)
        {
            if (paragraph != null)
            {
                results.Add(paragraph);
                paragraph = null;
            }
        }
    }
}