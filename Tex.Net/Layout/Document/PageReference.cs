using System;

namespace Tex.Net.Layout.Document
{
    public class PageReference : Leaf
    {
        public DocumentElement ReferencedElement { get; }

        public PageReference(DocumentElement referenced)
        {
            ReferencedElement = referenced;
        }

        public override LineNode[] GetNodes()
        {
            throw new NotImplementedException();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            throw new NotImplementedException();
        }
    }
}
