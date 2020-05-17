using Scriber.Text;

namespace Scriber.Layout.Document
{
    public class TextLeaf : Leaf, ITextLeaf
    {
        public string? Content { get; set; }

        public TextLeaf()
        {

        }

        public TextLeaf(string? content)
        {
            Content = content;
        }

        protected override AbstractElement CloneInternal()
        {
            return new TextLeaf(Content);
        }

        public override LineNode[] GetNodes()
        {
            if (Content == null)
            {
                throw new LayoutException("Content property is null");
            }

            return LineNodeTransformer.Create(this).ToArray();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Content == null)
            {
                throw new LayoutException("Content property is null");
            }
            if (Font == null)
            {
                throw new LayoutException("Font property is null");
            }

            var height = FontSize;
            var width = Font.GetWidth(Content, FontSize);
            return new Size(width, height);
        }
    }
}
