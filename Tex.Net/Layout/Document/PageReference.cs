namespace Tex.Net.Layout.Document
{
    public class PageReference : Leaf
    {
        public DocumentElement ReferencedElement { get; }

        public string PageNumber { get; set; } = "%";

        public PageReference()
        {
            ReferencedElement = this;
        }

        public PageReference(DocumentElement referenced)
        {
            ReferencedElement = referenced;
        }

        public override LineNode[] GetNodes()
        {
            return LineNodeTransformer.Create(this, PageNumber).ToArray();
        }

        public override void Interlude()
        {
            PageNumber = ReferencedElement.Page.Number;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var height = FontSize;
            var width = Font.GetWidth(PageNumber, FontSize);
            return new Size(width, height);
        }

        protected override DocumentElement CloneInternal()
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
