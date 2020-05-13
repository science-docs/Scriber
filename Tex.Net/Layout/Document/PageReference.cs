namespace Tex.Net.Layout.Document
{
    public class PageReference : Leaf, ITextLeaf
    {
        public AbstractElement ReferencedElement { get; }

        public string Content => PageNumber;

        public string PageNumber { get; set; } = "%";

        public PageReference()
        {
            ReferencedElement = this;
        }

        public PageReference(AbstractElement referenced)
        {
            ReferencedElement = referenced;
        }

        public override LineNode[] GetNodes()
        {
            return LineNodeTransformer.Create(this).ToArray();
        }

        public override void Interlude()
        {
            if (ReferencedElement.Page == null)
            {
                throw new LayoutException("Page property of the referenced element was not set");
            }

            PageNumber = ReferencedElement.Page.Number ?? throw new LayoutException("Pagenumber was not set");
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Font == null)
            {
                throw new LayoutException("Font property of the element was not set");
            }

            var height = FontSize;
            var width = Font.GetWidth(PageNumber, FontSize);
            return new Size(width, height);
        }

        protected override AbstractElement CloneInternal()
        {
            if (ReferencedElement == this)
            {
                return new PageReference()
                {
                    PageNumber = PageNumber
                };
            }
            else
            {
                return new PageReference(ReferencedElement.Clone())
                {
                    PageNumber = PageNumber
                };
            }
        }
    }
}
