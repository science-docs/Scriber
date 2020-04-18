using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tex.Net.Layout;

namespace Tex.Net.Drawing
{
    public interface IDrawingContext
    {
        Position Offset { get; set; }
        void DrawText(TextRun run, System.Drawing.Color color);
        void DrawImage(Image image, Rectangle rectangle);
    }
}
