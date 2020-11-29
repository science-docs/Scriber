using System;
using System.Collections.Generic;
using System.Linq;
using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Instructions
{
    public class BlockInstruction : EngineInstruction<ListSyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object Evaluate(CompilerState state, ListSyntax list)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var results = new List<Argument>();

            // Group continueous leafs into one paragraph
            Paragraph? currentParagraph = null;

            for (int i = 0; i < list.Count; i++)
            {
                var child = list[i];
                var flattened = EngineInstruction.Evaluate(state, child).Flatten();

                foreach (var element in flattened)
                {
                    if (element.Value is Leaf leaf)
                    {
                        if (currentParagraph == null)
                        {
                            currentParagraph = CreateNewParagraph(state);
                            results.Add(new Argument(element.Source, currentParagraph));
                        }

                        currentParagraph.Leaves.Add(leaf);
                    }
                    else if (element.Value is string str)
                    {
                        if (currentParagraph == null)
                        {
                            currentParagraph = CreateNewParagraph(state);
                            results.Add(new Argument(element.Source, currentParagraph));
                        }

                        currentParagraph.Leaves.Add(new TextLeaf(str));
                    }
                    else
                    {
                        ResetParagraph(state, ref currentParagraph);
                        results.Add(element);
                    }
                }
            }

            ResetParagraph(state, ref currentParagraph);

            return results.ToArray();
        }

        private Paragraph CreateNewParagraph(CompilerState state)
        {
            var currentParagraph = new Paragraph();
            var margin = currentParagraph.Margin;
            margin.Bottom = state.Document.Variable(ParagraphVariables.Skip);
            currentParagraph.FontSize = state.Document.Variable(FontVariables.FontSize);
            currentParagraph.Margin = margin;
            return currentParagraph;
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