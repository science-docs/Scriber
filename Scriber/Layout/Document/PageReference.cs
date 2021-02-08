namespace Scriber.Layout.Document
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

        protected override LineNode[] GetNodesOverride()
        {
            return LineNodeTransformer.Create(this).ToArray();
        }

        public override void Interlude()
        {
            PageNumber = ReferencedElement.Page?.Number ?? throw new LayoutException("Pagenumber was not set");
            Invalidate();
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
