using System.Collections.Generic;
using System.Linq;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine
{
    public class BlockInstruction : EngineInstruction
    {
        public override object Execute(CompilerState state, object[] arguments)
        {
            var results = new List<object>();

            // Group continueous leafs into one paragraph
            Paragraph currentParagraph = null;

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
                else
                {
                    if (currentParagraph != null)
                    {
                        results.Add(currentParagraph);
                        currentParagraph = null;
                    }
                    results.Add(item);
                }
            }

            if (currentParagraph != null)
            {
                results.Add(currentParagraph);
            }

            return results.ToArray();
        }
    }
}