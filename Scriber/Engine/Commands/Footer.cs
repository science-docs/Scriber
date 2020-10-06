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
            name ??= LabelVariables.FootnoteCounter.Increment(state.Document).ToString();
            var footnote = new FootnoteLeaf(name, content);
            content.Parent = footnote;
            content.HorizontalAlignment = Layout.HorizontalAlignment.Justify;
            var footnoteSize = state.Document.Variable(FontVariables.FootnoteSize);
            if (footnoteSize != null)
            {
                content.FontSize = footnoteSize.Value;
            }
            
            return footnote;
        }

        [Command("CenterHeader")]
        public static void CenterHeader(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.TopCenter);
        }

        [Command("LeftHeader")]
        public static void LeftHeader(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.TopLeft);
        }

        [Command("RightHeader")]
        public static void RightHeader(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.TopRight);
        }

        [Command("CenterFooter")]
        public static void CenterFooter(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.BottomCenter);
        }

        [Command("LeftFooter")]
        public static void LeftFooter(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.BottomLeft);
        }

        [Command("RightFooter")]
        public static void RightFooter(CompilerState state, DocumentElement content)
        {
            SetFixedBlock(state, content, FixedPosition.BottomRight);
        }

        private static void SetFixedBlock(CompilerState state, DocumentElement content, FixedPosition position)
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
