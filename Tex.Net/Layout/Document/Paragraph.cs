using System;
using System.Collections.Generic;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public class Paragraph : Block
    {
        public DocumentElementCollection<Leaf> Leaves { get; }

        private List<LineNode> lineNodes;
        private PositionedItem[] positionedItems;

        public Paragraph()
        {
            Leaves = new DocumentElementCollection<Leaf>(this);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            lineNodes = new List<LineNode>();
            foreach (var leaf in Leaves)
            {
                leaf.Measure(availableSize);
                lineNodes.AddRange(leaf.GetNodes());
            }
            var linebreak = new Linebreak();
            var breaks = linebreak.BreakLines(lineNodes, new double[] { availableSize.Width }, new LinebreakOptions());
            positionedItems = linebreak.PositionItems(lineNodes, new double[] { availableSize.Width }, breaks, false);
            var last = positionedItems[^1];
            var lineheight = 10.0;
            return new Size(availableSize.Width, lineheight * last.Line + Leaves[^1].DesiredSize.Height);
        }

        protected override void ArrangeOverride(Rectangle finalRectangle)
        {
            throw new NotImplementedException();
        }

        public override Block[] Split()
        {
            base.Split();

            List<ParagraphLine> lines = new List<ParagraphLine>();
            var curItems = new List<PositionedItem>();
            var curLine = 0;
            foreach (var item in positionedItems)
            {
                if (item.Line != curLine)
                {
                    lines.Add(new ParagraphLine(lineNodes, curItems));
                    curItems.Clear();
                    curLine = item.Line;
                }
                curItems.Add(item);
            }

            if (curItems.Count > 0)
            {
                lines.Add(new ParagraphLine(lineNodes, curItems));
            }

            return lines.ToArray();
        }

        public override void OnRender(IDrawingContext drawingContext)
        {
            throw new InvalidOperationException("A paragraph should never be rendered");
        }
    }
}
