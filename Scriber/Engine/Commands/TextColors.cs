using System.Collections.Generic;
using Scriber.Layout.Document;
using Scriber.Text;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class TextColors
    {
        [Command("Color")]
        public static IEnumerable<Leaf> ColorText(CompilerState state, string colorName, IEnumerable<Leaf> leaves)
        {
            var color = Color.FromCode(colorName);

            if (color == null)
            {
                color = Color.FromName(colorName);
                if (color == null)
                {
                    state.Issues.Add(state.CurrentElement, CompilerIssueType.Warning, $"Cannot find color with name '{colorName}'. Applying default color.");
                    return leaves;
                }
            }
            return ApplyColor(color.Value, leaves);
        }

        [Command("Red")]
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
