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

        protected override LineNode[] GetNodesOverride()
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

        protected override AbstractElement CloneInternal()
        {
            return new ReferenceLeaf(Referenced, Preamble);
        }
    }
}
