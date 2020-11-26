using System;
using System.Collections.Generic;
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
        public override object Execute(CompilerState state, ListSyntax list)
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

            for (int i = 0; i < list.Children.Count; i++)
            {
                var child = list.Children[i];
                var flattened = EngineInstruction.Execute(state, child).Flatten();

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
                    //else if (item.Value == EmptyInstruction.Object)
                    //{
                    //    ResetParagraph(state, ref currentParagraph);
                    //}
                    //else if (item.Value == NullInstruction.NullObject)
                    //{
                    //    ResetParagraph(state, ref currentParagraph);
                    //    results.Add(new Argument(item.Source, null));
                    //}
                    else
                    {
                        ResetParagraph(state, ref currentParagraph);
                        results.Add(element);
                    }
                }

                
            }

            ResetParagraph(state, ref currentParagraph);

            if (results.Count == 0)
            {
                // signaling an empty block
                results.Add(new Argument(list, null));
                state.Context.Logger.Debug("Empty explicit block found. Adding null element");
            }

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