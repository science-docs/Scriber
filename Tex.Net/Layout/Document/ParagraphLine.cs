using System;
using System.Collections.Generic;
using System.Drawing;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public class ParagraphLine : Block
    {
        public List<LineNode> LineNodes { get; }
        public List<PositionedItem> PositionedItems { get; }

        public ParagraphLine(List<LineNode> nodes, IEnumerable<PositionedItem> items)
        {
            LineNodes = nodes;
            PositionedItems = new List<PositionedItem>(items);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size();
        }

        protected override void ArrangeOverride(Rectangle finalRectangle)
        {

        }

        public override void OnRender(IDrawingContext drawingContext)
        {
            foreach (var item in PositionedItems)
            {
                var node = LineNodes[item.Index];
                var offset = new Position(VisualOffset.X + item.Offset, VisualOffset.Y);

                var run = new TextRun
                {
                    Text = node.Text,
                    Typeface = new Net.Text.Typeface
                    {
                        Font = node.Font,
                        Size = node.Size
                    }
                };

                drawingContext.Offset = offset;

                drawingContext.DrawText(run, Color.Red);
            }
        }
    }
}
