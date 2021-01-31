using System;
using System.Collections.Generic;
using Scriber.Autocomplete;
using Scriber.Drawing;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package("Colors")]
    public static class TextColors
    {
        [Command("DefineColor")]
        public static void DefineColor(CompilerState state, Argument<string> colorName, Argument<string> value)
        {
            if (string.IsNullOrWhiteSpace(colorName.Value))
            {
                state.Issues.Add(value.Source, CompilerIssueType.Warning, "Invalid name for color.");
            }

            var color = Color.FromCode(value.Value);

            if (color != null)
            {
                var customColors = ColorVariables.CustomColors.Get(state.Document) ?? throw new InvalidOperationException();
                customColors[colorName.Value] = color.Value;
            }
            else
            {
                state.Issues.Add(value.Source, CompilerIssueType.Warning, $"Could not create color from '{value.Value}'.");
            }
        }

        [Command("Color")]
        public static IEnumerable<Leaf> ColorText(CompilerState state, [Argument(ProposalProvider = typeof(ColorProposalProvider))] Argument<string> colorName, IEnumerable<Leaf> leaves)
        {
            var color = Color.FromCode(colorName.Value);

            if (color == null)
            {
                color = Color.FromName(colorName.Value);
                if (color == null)
                {
                    state.Issues.Add(colorName.Source, CompilerIssueType.Warning, $"Cannot find color with name '{colorName}'. Applying no color.");
                    return leaves;
                }
            }
            return ApplyColor(color.Value, leaves);
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
