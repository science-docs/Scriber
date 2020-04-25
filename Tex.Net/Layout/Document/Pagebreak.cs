using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public class Pagebreak : DocumentElement
    {
        public override bool IsVisible => false;

        protected override AbstractElement CloneInternal()
        {
            return new Pagebreak();
        }

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
        }

        protected override Measurements MeasureOverride(Size availableSize)
        {
            // forces a pagebreak by setting infinity as the height
            return Measurements.Singleton(new Measurement(this, new Size(availableSize.Width, double.PositiveInfinity), Thickness.Zero));
        }
    }
}
