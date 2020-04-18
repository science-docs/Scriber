using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public interface IBlock
    {
        Size DesiredSize { get; }
        Size RenderSize { get; }
        Position VisualOffset { get; }
        Size Measure(Size availableSize);
        IBlock[] Split();
        void Arrange(Rectangle finalRectangle);
        void OnRender(IDrawingContext drawingContext);
    }
}
