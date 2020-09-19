using System;
using System.Collections.Generic;
using System.Linq;
using Scriber.Drawing;
using Scriber.Variables;

namespace Scriber.Layout.Document
{
    public class Paragraph : DocumentElement
    {
        public ElementCollection<Leaf> Leaves { get; }

        private List<LineNode>? lineNodes;
        private PositionedItem[][]? lines;

        public Paragraph()
        {
            Leaves = new ElementCollection<Leaf>(this);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            var doc = Document ?? throw new LayoutException("Document was not set");

            var ms = new Measurement(this);
            lineNodes = new List<LineNode>();
            foreach (var leaf in Leaves)
            {
                leaf.Measure(availableSize);
                var nodes = leaf.GetNodes();
                if (nodes.Length > 0)
                {
                    lineNodes.AddRange(nodes);
                }
            }
            if (lineNodes.Count > 0)
            {
                lineNodes.Add(LineNode.Glue(0, 0, 0));
            }
            for (int i = 0; i < lineNodes.Count - 1; i++)
            {
                if (lineNodes[i].Type == LineNodeType.Glue && lineNodes[i + 1].Type == LineNodeType.Glue)
                {
                    lineNodes.RemoveAt(i);
                }
            }
            if (lineNodes.Count > 0)
            {
                var linebreak = new Linebreak();
                var breaks = linebreak.BreakLines(lineNodes, new double[] { availableSize.Width }, new LinebreakOptions());
                var positionedItems = linebreak.PositionItems(lineNodes, new double[] { availableSize.Width }, breaks, false);
                lines = ProduceLines(positionedItems);
                double totalHeight = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var last = i == lines.Length - 1;
                    var line = lines[i];
                    var height = 0.0;
                    var lastElement = line[^1];
                    var width = lastElement.Offset;
                    var lastNode = lineNodes[lastElement.Index];
                    width += lastNode.Width;
                    foreach (var item in line)
                    {
                        height = Math.Max(lineNodes[item.Index].Element?.DesiredSize.Height ?? 0, height);
                    }

                    var stretch = ParagraphVariables.BaselineStretch[doc];

                    if (!last)
                    {
                        height *= stretch;
                    }
                    var size = new Size(width, height);
                    var measurement = new Measurement(this, size, Thickness.Zero)
                    {
                        Tag = line,
                        Position = new Position(0, totalHeight)
                    };
                    totalHeight += height;
                    ms.Subs.Add(measurement);

                    foreach (var node in line)
                    {
                        var item = lineNodes[node.Index];
                        if (item.Element is FootnoteLeaf footnote)
                        {
                            var footMS = footnote.Element.Measure(Document.PageBoxSize);
                            measurement.Extra.Subs.Add(footMS);
                        }
                    }
                }
            }

            
            if (ms.Subs.Count > 0)
            {
                ms.Margin = new Thickness(Margin.Top, 0, Margin.Bottom, 0);
            }

            return ms;
        }

        private static PositionedItem[][] ProduceLines(PositionedItem[] items)
        {
            List<PositionedItem[]> lines = new List<PositionedItem[]>();
            var curItems = new List<PositionedItem>();
            var curLine = 0;
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.Line != curLine)
                    {
                        lines.Add(curItems.ToArray());
                        curItems.Clear();
                        curLine = item.Line;
                    }
                    curItems.Add(item);
                }
            }

            if (curItems.Count > 0)
            {
                lines.Add(curItems.ToArray());
            }

            return lines.ToArray();
        }

        public override SplitResult Split(Measurement source, double height)
        {
            var measurement = new Measurement(this);
            Measurement? next = new Measurement(this);
            double carryOver = -1;

            foreach (var sub in source.Subs)
            {
                var pos = sub.Position;
                var size = sub.TotalSize + sub.AccumulatedExtra.TotalSize;
                if (pos.Y + size.Height >= height)
                {
                    if (carryOver == -1)
                    {
                        carryOver = height - sub.Position.Y;
                    }

                    if (sub.Tag is PositionedItem[] items)
                    {
                        next.Extra.Subs.AddRange(GetFootnotes(items));
                    }

                    pos.Y -= height;
                    pos.Y += carryOver;
                    sub.Position = pos;
                    next.Subs.Add(sub);
                }
                else
                {
                    if (sub.Tag is PositionedItem[] items)
                    {
                        var footnotes = GetFootnotes(items);
                        foreach (var footMeasurement in footnotes)
                        {
                            height -= footMeasurement.TotalSize.Height;
                        }
                        measurement.Extra.Subs.AddRange(footnotes);
                    }

                    measurement.Subs.Add(sub);
                }
            }

            if (next.Subs.Count == 0)
            {
                next = null;
            }

            return new SplitResult(source, measurement, next);
        }

        private IEnumerable<Measurement> GetFootnotes(PositionedItem[] items)
        {
            foreach (var item in items)
            {
                var node = lineNodes![item.Index];
                if (node.Element is FootnoteLeaf footnote)
                {
                    yield return footnote.Element.Measurement;
                }
            }
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            if (lines == null || lineNodes == null)
            {
                throw new InvalidOperationException("Rendering has been called before measuring");
            }

            foreach (var sub in measurement.Subs)
            {
                if (sub.Tag is PositionedItem[] items)
                {
                    foreach (var item in items)
                    {
                        var node = lineNodes[item.Index];

                        if (node.Element == null)
                        {
                            throw new NullReferenceException("The Element property of a LineNode was null");
                        }

                        var offset = new Position(item.Offset, sub.Position.Y);

                        var run = RunFromNode(node);

                        if (lineNodes.Count > item.Index + 1)
                        {
                            var next = lineNodes[item.Index + 1];
                            if (next.Type == LineNodeType.Glue)
                            {
                                run.Text += " ";
                            }
                        }

                        drawingContext.Offset = offset;
                        drawingContext.DrawText(run, node.Element.Foreground);
                    }
                }
            }
        }

        private static TextRun RunFromNode(LineNode node)
        {
            if (node.Element == null)
            {
                throw new NullReferenceException("The Element property of a LineNode was null");
            }

            if (node.Element.Font == null)
            {
                throw new NullReferenceException("The Font property of a LineNode was null");
            }

            return new TextRun(node.Text ?? string.Empty, new Text.Typeface(node.Element.Font, node.Element.FontSize, node.Element.FontWeight, node.Element.FontStyle));
        }


        public override void Interlude()
        {
            foreach (var leaf in Leaves)
            {
                leaf.Interlude();
            }
        }

        protected override AbstractElement CloneInternal()
        {
            var paragraph = new Paragraph();

            foreach (var leaf in Leaves.Where(e => !(e is FootnoteLeaf)))
            {
                paragraph.Leaves.Add(leaf.Clone());
            }

            return paragraph;
        }

        public new Paragraph Clone()
        {
            if (!(base.Clone() is Paragraph paragraph))
            {
                throw new InvalidCastException();
            }

            return paragraph;
        }

        public static Paragraph FromText(string text)
        {
            return FromLeaves(new TextLeaf(text));
        }

        public static Paragraph FromLeaves(params Leaf[] leaves)
        {
            var paragraph = new Paragraph();
            paragraph.Leaves.AddRange(leaves);
            return paragraph;
        }
    }
}
