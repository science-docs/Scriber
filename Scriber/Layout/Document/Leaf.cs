using Scriber.Layout.Styling;

namespace Scriber.Layout.Document
{
    public abstract class Leaf : AbstractElement
    {
        public double DesiredHeight { get; private set; }

        private LineNode[]? lineNodeCache;

        public LineNode[] GetNodes()
        {
            if (lineNodeCache != null && IsValid)
            {
                return lineNodeCache;
            }

            IsValid = true;
            lineNodeCache = GetNodesOverride();
            return lineNodeCache;
        }

        protected abstract LineNode[] GetNodesOverride();

        public double Measure(Size availableSize)
        {
            if (IsValid)
            {
                return DesiredHeight;
            }

            return DesiredHeight = MeasureOverride(availableSize);
        }

        protected virtual double MeasureOverride(Size availableSize)
        {
            return Style.Get(StyleKeys.FontSize).Point;
        }

        public new Leaf Clone()
        {
            return Clone<Leaf>();
        }
    }
}
