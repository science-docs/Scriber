namespace Scriber.Layout.Document
{
    public class ReferenceLeaf : Leaf, ITextLeaf
    {
        public AbstractElement Referenced { get; set; }
        public string Preamble { get; set; }

        public string? Content => Preamble;

        public ReferenceLeaf(AbstractElement referenced, string preamble)
        {
            Referenced = referenced;
            Preamble = preamble;
        }

        public override LineNode[] GetNodes()
        {
            var items = LineNodeTransformer.Create(this);
            if (Referenced.Page != null)
            {
                foreach (var item in items)
                {
                    item.Link = Referenced.Page.Index;
                }
            }
            
            return items.ToArray();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Font == null)
            {
                throw new LayoutException("Font property of the element was not set");
            }

            var height = FontSize;
            var width = Font.GetWidth(Preamble, FontSize, FontWeight);
            return new Size(width, height);
        }

        protected override AbstractElement CloneInternal()
        {
            return new ReferenceLeaf(Referenced, Preamble);
        }
    }
}
