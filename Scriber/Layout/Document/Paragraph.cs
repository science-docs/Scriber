using System;
using System.Collections.Generic;
using System.Linq;
using Scriber.Drawing;
using Scriber.Layout.Styling;
using Scriber.Text;
using Scriber.Variables;

namespace Scriber.Layout.Document
{
    public class Paragraph : DocumentElement
    {
        public ElementCollection<Leaf> Leaves { get; }

        private List<LineNode>? lineNodes;
        private PositionedItem[][]? lines;

        public override IEnumerable<Symbol> Symbols => Leaves.SelectMany(e => e.Symbols);

        public Paragraph()
        {
            Tag = "p";
            Leaves = new ElementCollection<Leaf>(this);
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            return Leaves;
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
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
            LineNodeTransformer.Clean(lineNodes);
            if (lineNodes.Count > 0)
            {
                var indent = Style.Get(StyleKeys.ParagraphIndent);

                if (indent.Value > 0)
                {
                    lineNodes.Insert(0, LineNode.Box(indent.Point, string.Empty));
                }

                var linebreak = new Linebreak();
                var breaks = linebreak.BreakLines(lineNodes, new double[] { availableSize.Width }, new LinebreakOptions());
                var positionedItems = linebreak.PositionItems(lineNodes, new double[] { availableSize.Width }, breaks, false);
                lines = ProduceLines(positionedItems);
                double totalHeight = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var height = 0.0;
                    var lastElement = line[^1];
                    var width = lastElement.Offset;
                    var lastNode = lineNodes[lastElement.Index];
                    width += lastNode.Width;
                    foreach (var item in line)
                    {
                        height = Math.Max(lineNodes[item.Index].Element?.DesiredHeight ?? 0, height);
                    }

                    var skip = Style.Get(StyleKeys.BaselineSkip);
                    if (skip.Value < 0)
                    {
                        skip = Unit.FromPoint(height);
                    }
                    var stretch = Style.Get(StyleKeys.BaselineStretch);

                    height = skip.Point * stretch;
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
                            var footMS = footnote.Element.Measure(Document!.Variable(PageVariables.BoxSize));
                            measurement.Extra.Subs.Add(footMS);
                        }
                    }
                }
            }

            if (ms.Subs.Count > 0)
            {
                ms.Margin = Style.Get(StyleKeys.FullMargin);
            }

            return ms;
        }

        private static PositionedItem[][] ProduceLines(PositionedItem[] items)
        {
            List<PositionedItem[]> lines = new();
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
            var measurement = new Measurement(this, null, source.Margin);
            Measurement? next = new(this, null, source.Margin);
            double carryOver = -1;

            // Never try to split less lines than the threshold specifies
            const int widowLineThreshold = 2;
            if (source.Subs.Count < widowLineThreshold * 2 ||
                source.Subs.Take(widowLineThreshold).Sum(e => e.TotalSize.Height + e.AccumulatedExtra.TotalSize.Height) > height)
            {
                return SplitResult.Fail(source);
            }

            for (int i = 0; i < source.Subs.Count; i++)
            {
                var sub = source.Subs[i];
                var pos = sub.Position;
                var size = sub.TotalSize + sub.AccumulatedExtra.TotalSize;
                if (pos.Y + size.Height >= height || i >= source.Subs.Count - widowLineThreshold)
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
                return SplitResult.Fail(source);
            }
            else
            {
                Invalidate();
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

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            if (lineNodes == null)
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
                            continue;
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

                        if (node.Link != null)
                        {
                            var rect = new Rectangle(offset, new Size(node.Width, node.Element.Style.Get(StyleKeys.FontSize).Point));
                            drawingContext.AddLink(rect, node.Link.Value);
                        }
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
            var typeface = node.Element.Style.Get(StyleKeys.Typeface);
            return new TextRun(node.Text ?? string.Empty, typeface);
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
            return Clone<Paragraph>();
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
