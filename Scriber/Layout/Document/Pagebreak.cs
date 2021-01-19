using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class Pagebreak : DocumentElement
    {
        protected override AbstractElement CloneInternal()
        {
            return new Pagebreak();
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            // forces a pagebreak by setting infinity as the height
            return new Measurement(this, new Size(availableSize.Width, double.PositiveInfinity), Thickness.Zero);
        }
    }
}
