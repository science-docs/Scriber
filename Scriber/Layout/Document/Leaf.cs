using System;

namespace Scriber.Layout.Document
{
    public abstract class Leaf : AbstractElement
    {
        public Size DesiredSize { get; private set; }

        public abstract LineNode[] GetNodes();

        public Size Measure(Size availableSize)
        {
            return DesiredSize = MeasureOverride(availableSize);
        }

        protected abstract Size MeasureOverride(Size availableSize);

        public new Leaf Clone()
        {
            if (!(base.Clone() is Leaf leaf))
            {
                throw new InvalidCastException($"Cloned element is not of type {nameof(Leaf)}");
            }

            return leaf;
        }
    }
}
