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
                    ResetParagraph(state, results, ref currentParagraph);
                }
                else if (item == NullInstruction.NullObject)
                {
                    ResetParagraph(state, results, ref currentParagraph);
                    results.Add(null);
                }
                else
                {
                    ResetParagraph(state, results, ref currentParagraph);
                    results.Add(item);
                }
            }

            ResetParagraph(state, results, ref currentParagraph);

            if (results.Count == 0 && Origin.Type == ElementType.ExplicitBlock)
            {
                // signaling an empty block
                results.Add(null);
                state.Issues.Log(Origin, "Empty explicit block found. Adding null element");
            }

            return results.ToArray();
        }

        private void ResetParagraph(CompilerState state, List<object?> results, ref Paragraph? paragraph)
        {
            if (paragraph != null)
            {
                state.Issues.Log(Origin, $"Grouping {paragraph.Leaves.Count} {LeafString(paragraph.Leaves.Count)} into a paragraph.");
                results.Add(paragraph);
                paragraph = null;
            }
        }

        private static string LeafString(int count)
        {
            return count == 1 ? "leaf" : "leaves";
        }
    }
}