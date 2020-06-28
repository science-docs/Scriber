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

        public override object Execute(CompilerState state, Argument[] arguments)
        {
            var results = new List<Argument>();

            // Group continueous leafs into one paragraph
            Paragraph? currentParagraph = null;

            for (int i = 0; i < arguments.Length; i++)
            {
                var item = arguments[i];
                if (item.Value is Leaf leaf)
                {
                    if (currentParagraph == null)
                    {
                        currentParagraph = new Paragraph();
                        var margin = currentParagraph.Margin;
                        margin.Bottom = state.Document.Variables["length", "parskip"].GetValue<double>();
                        currentParagraph.Margin = margin;
                        results.Add(new Argument(item.Source, currentParagraph));
                    }

                    currentParagraph.Leaves.Add(leaf);
                }
                else if (item == EmptyInstruction.Object)
                {
                    ResetParagraph(state, ref currentParagraph);
                }
                else if (item == NullInstruction.NullObject)
                {
                    ResetParagraph(state, ref currentParagraph);
                    results.Add(new Argument(item.Source, null));
                }
                else
                {
                    ResetParagraph(state, ref currentParagraph);
                    results.Add(item);
                }
            }

            ResetParagraph(state, ref currentParagraph);

            if (results.Count == 0 && Origin.Type == ElementType.ExplicitBlock)
            {
                // signaling an empty block
                results.Add(new Argument(Origin, null));
                state.Context.Logger.Debug("Empty explicit block found. Adding null element");
            }

            return results.ToArray();
        }

        private void ResetParagraph(CompilerState state, ref Paragraph? paragraph)
        {
            if (paragraph != null)
            {
                state.Context.Logger.Debug($"Grouping {paragraph.Leaves.Count} {LeafString(paragraph.Leaves.Count)} into a paragraph.");
                paragraph = null;
            }
        }

        private static string LeafString(int count)
        {
            return count == 1 ? "leaf" : "leaves";
        }
    }
}