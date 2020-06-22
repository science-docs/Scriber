using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class TextWeights
    {
        [Command("Bold")]
        public static IEnumerable<Leaf> Bold(IEnumerable<Leaf> leaves)
        {
            foreach (var leaf in leaves)
            {
                leaf.FontWeight = Text.FontWeight.Bold;
            }

            return leaves;
        }
    }
}
