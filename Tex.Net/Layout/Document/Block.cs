using System;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public abstract class Block : IBlock
    {
        public Size DesiredSize { get; private set; }
        public Size RenderSize { get; private set; }
        public Position VisualOffset { get; private set; }

        private bool measured = false;
        private bool split = false;
        private bool arranged = false;

        public Size Measure(Size availableSize)
        {
            measured = true;
            return DesiredSize = MeasureOverride(availableSize);
        }

        public void Arrange(Rectangle finalRectangle)
        {
            VisualOffset = finalRectangle.Position;
            RenderSize = finalRectangle.Size;
            ArrangeOverride(finalRectangle);
        }

        protected abstract Size MeasureOverride(Size availableSize);

        protected abstract void ArrangeOverride(Rectangle finalRectangle);

        public abstract void OnRender(IDrawingContext drawingContext);

        public virtual IBlock[] Split()
        {
            if (!measured)
            {
                throw new InvalidOperationException("Before splitting, an object has to be measured");
            }

            return new IBlock[] { this };
        }

        
    }
}
