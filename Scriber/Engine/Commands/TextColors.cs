using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Scriber.Layout.Document;
using Scriber.Text;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class TextColors
    {
        [Command("color")]
        public static IEnumerable<Leaf> ColorText(string colorName, IEnumerable<Leaf> leaves)
        {
            var color = Color.FromName(colorName) ?? Colors.Black;
            return ApplyColor(color, leaves);
        }

        [Command("red")]
        public static IEnumerable<Leaf> Red(IEnumerable<Leaf> leaves)
        {
            return ApplyColor(Colors.Red, leaves);
        }

        private static IEnumerable<Leaf> ApplyColor(Color color, IEnumerable<Leaf> leaves)
        {
            foreach (var leaf in leaves)
            {
                leaf.Foreground = color;
            }
            return leaves;
        }
    }
}
