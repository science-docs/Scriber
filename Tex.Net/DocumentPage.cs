using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Drawing;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;

namespace Tex.Net
{
    public class DocumentPage
    {
        public List<Block> Blocks { get; } = new List<Block>();
        public Size Size { get; set; }
        public Rectangle ContentArea { get; set; }

        public void OnRender(IDrawingContext drawingContext)
        {
            foreach (var block in Blocks)
            {
                drawingContext.Offset = block.VisualOffset;
                block.OnRender(drawingContext);
            }
        }
    }
}
