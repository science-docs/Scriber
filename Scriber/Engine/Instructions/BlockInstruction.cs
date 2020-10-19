using System;
using System.Collections.Generic;
using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Instructions
{
    public class BlockInstruction : EngineInstruction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        public BlockInstruction(Element origin) : base(origin)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public override object Execute(CompilerState state, Argument[] arguments)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

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
                        currentParagraph = CreateNewParagraph(state);
                        results.Add(new Argument(item.Source, currentParagraph));
                    }

                    currentParagraph.Leaves.Add(leaf);
                }
                else if (item.Value is string str)
                {
                    if (currentParagraph == null)
                    {
                        currentParagraph = CreateNewParagraph(state);
                        results.Add(new Argument(item.Source, currentParagraph));
                    }

                    currentParagraph.Leaves.Add(new TextLeaf(str));
                }
                else if (item.Value == EmptyInstruction.Object)
                {
                    ResetParagraph(state, ref currentParagraph);
                }
                else if (item.Value == NullInstruction.NullObject)
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