using Scriber.Layout.Styling;
using System;

namespace Scriber.Layout.Document
{
    public abstract class Leaf : AbstractElement
    {
        public double DesiredHeight { get; private set; }

        public abstract LineNode[] GetNodes();

        public double Measure(Size availableSize)
        {
            if (IsValid)
            {
                return DesiredHeight;
            }

            IsValid = true;
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
