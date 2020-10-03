using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Footer
    {
        [Command("Footnote")]
        public static FootnoteLeaf Footnote(CompilerState state, Paragraph content, string? name = null)
        {
            name ??= "0";
            var footnote = new FootnoteLeaf(name, content);
            content.HorizontalAlignment = Layout.HorizontalAlignment.Justify;
            var footnoteSize = state.Document.Variable(FontVariables.FootnoteSize);
            if (footnoteSize != null)
            {
                content.FontSize = footnoteSize.Value;
            }
            content.Parent = footnote;
            return footnote;
        }

        [Command("CenterFooter")]
        public static void CenterFooter(CompilerState state, Paragraph content)
        {
            SetFixedBlock(state, content, FixedPosition.BottomCenter);
        }

        private static void SetFixedBlock(CompilerState state, Paragraph content, FixedPosition position)
        {
            var block = new FixedBlock(content)
            {
                Position = position
            };

            for (int i = 0; i < state.Document.PageItems.Count; i++)
            {
                var item = state.Document.PageItems[i];
                if (item is FixedBlock fix && fix.Position == position)
                {
                    state.Document.PageItems.RemoveAt(i);
                    break;
                }
            }

            state.Document.PageItems.Add(block);
        }
    }
}
