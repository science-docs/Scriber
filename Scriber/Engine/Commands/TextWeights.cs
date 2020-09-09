using Scriber.Layout.Document;
using System;
using System.Collections.Generic;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class TextWeights
    {
        [Command("Bold")]
        public static IEnumerable<Leaf> Bold(IEnumerable<Leaf> leaves)
        {
            return leaves.Apply(e => e.FontWeight = Text.FontWeight.Bold);
        }

        [Command("Italic")]
        public static IEnumerable<Leaf> Italic(IEnumerable<Leaf> leaves)
        {
            return leaves.Apply(e => e.FontWeight = Text.FontWeight.Italic);
        }

        [Command("BoldItalic")]
        public static IEnumerable<Leaf> BoldItalic(IEnumerable<Leaf> leaves)
        {
            return leaves.Apply(e => e.FontWeight = Text.FontWeight.BoldItalic);
        }

        private static IEnumerable<Leaf> Apply(this IEnumerable<Leaf> leaves, Action<Leaf> action)
        {
            foreach (var leaf in leaves)
            {
                action(leaf);
            }

            return leaves;
        }
    }
}
