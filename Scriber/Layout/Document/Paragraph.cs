using System;
using System.Collections.Generic;
using Scriber.Drawing;

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

        protected override Measurements MeasureOverride(Size availableSize)
        {
            var doc = Document ?? throw new LayoutException("Document was not set");

            var ms = new Measurements();
            lineNodes = new List<LineNode>();
            foreach (var leaf in Leaves)
            {
                leaf.Measure(availableSize);
                var nodes = leaf.GetNodes();
                if (nodes.Length > 0)
                {
                    lineNodes.AddRange(nodes);
                    //lineNodes.Add(LineNodeTransformer.GetDefaultGlue(leaf));
                }
            }
            for (int i = 0; i < lineNodes.Count - 1; i++)
            {
                if (lineNodes[i].Type == LineNodeType.Glue && lineNodes[i + 1].Type == LineNodeType.Glue)
                {
                    lineNodes.RemoveAt(i);
                }
            }
            if (lineNodes.Count > 1)
            {
                //lineNodes.RemoveAt(lineNodes.Count - 1);
                var linebreak = new Linebreak();
                var breaks = linebreak.BreakLines(lineNodes, new double[] { availableSize.Width }, new LinebreakOptions());
                var positionedItems = linebreak.PositionItems(lineNodes, new double[] { availableSize.Width }, breaks, false);
                lines = ProduceLines(positionedItems);

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

                    var stretch = doc.Variables[DocumentVariables.Length, DocumentVariables.BaselineStretch].GetValue<double>();

                    if (!last)
                    {
                        height *= stretch;
                    }
                    var size = new Size(width, height);
                    var measurement = new Measurement(this, size, Thickness.Zero);
                    ms.Add(measurement);

                    foreach (var node in line)
                    {
                        var item = lineNodes[node.Index];
                        if (item.Element is FootnoteLeaf footnote)
                        {
                            var footMS = footnote.Element.Measure(Document.PageBoxSize);
                            ms[^1].PagebreakPenalty = double.PositiveInfinity;
                            ms.AddInternal(footMS);
                            foreach (var m in footMS)
                            {
                                m.PagebreakPenalty = double.PositiveInfinity;
                            }
                        }
                    }
                }
            }

            
            if (ms.Count > 0)
            {
                ms[0].Margin = new Thickness(Margin.Top, 0, 0, 0);
                ms[^1].Margin = new Thickness(0, 0, Margin.Bottom, 0);

                if (ms.Count > 1)
                {
                    ms[^2].PagebreakPenalty = double.PositiveInfinity;
                }
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

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            if (lines == null || lineNodes == null)
            {
                throw new InvalidOperationException("Rendering has been called before measuring");
            }

            var items = lines[measurement.Index];
            var position = measurement.Position;

            foreach (var item in items)
            {
                var node = lineNodes[item.Index];

                if (node.Element == null)
                {
                    throw new NullReferenceException("The Element property of a LineNode was null");
                }

                var offset = new Position(position.X + item.Offset, position.Y);

                var run = RunFromNode(node);

                drawingContext.Offset = offset;
                drawingContext.DrawText(run, node.Element.Foreground);
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

            foreach (var leaf in Leaves)
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
    }
}
