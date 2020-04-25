namespace Tex.Net.Layout.Document
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
            return base.Clone() as Leaf;
        }
    }
}
