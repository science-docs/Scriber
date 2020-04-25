using Tex.Net.Text;

namespace Tex.Net.Layout.Document
{
    public class TextLeaf : Leaf, ITextLeaf
    {
        public string Content { get; set; }

        protected override AbstractElement CloneInternal()
        {
            return new TextLeaf { Content = Content };
        }

        public override LineNode[] GetNodes()
        {
            return LineNodeTransformer.Create(this, Content).ToArray();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var height = FontSize;
            var width = Font.GetWidth(Content, FontSize);
            return new Size(width, height);
        }
    }
}
