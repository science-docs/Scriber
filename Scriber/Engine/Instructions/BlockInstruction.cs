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

            foreach (var element in list.SelectMany(e => EngineInstruction.Evaluate(state, e).Flatten()))
            {
                if (element.Value is Leaf leaf)
                {
                    if (currentParagraph == null)
                    {
                        currentParagraph = new Paragraph();
                        results.Add(new Argument(element.Source, currentParagraph));
                    }

                    currentParagraph.Leaves.Add(leaf);
                }
                else if (element.Value is string str)
                {
                    if (currentParagraph == null)
                    {
                        currentParagraph = new Paragraph();
                        results.Add(new Argument(element.Source, currentParagraph));
                    }

                    currentParagraph.Leaves.Add(new TextLeaf(str));
                }
                else if (element.Value != null)
                {
                    currentParagraph = null;
                    results.Add(element);
                }
            }

            return results.ToArray();
        }
    }
}