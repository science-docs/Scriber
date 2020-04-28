﻿using System;
using System.Collections.Generic;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
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
                    lineNodes.Add(LineNodeTransformer.GetDefaultGlue(leaf));
                }
            }
            if (lineNodes.Count > 1)
            {
                lineNodes.RemoveAt(lineNodes.Count - 1);
                var linebreak = new Linebreak();
                var breaks = linebreak.BreakLines(lineNodes, new double[] { availableSize.Width }, new LinebreakOptions());
                var positionedItems = linebreak.PositionItems(lineNodes, new double[] { availableSize.Width }, breaks, false);
                lines = ProduceLines(positionedItems);

                for (int i = 0; i < lines.Length; i++)
                {
                    var first = i == 0;
                    var last = i == lines.Length - 1;
                    var line = lines[i];
                    var height = 0.0;
                    foreach (var item in line)
                    {
                        height = Math.Max(lineNodes[item.Index].Element.DesiredSize.Height, height);
                    }
                    var stretch = doc.Variables[DocumentVariables.Length][DocumentVariables.BaselineStretch].GetValue<double>();

                    var margin = new Thickness();
                    if (first)
                    {
                        margin.Top = Margin.Top;
                    }
                    if (last)
                    {
                        margin.Bottom = Margin.Bottom;
                    }
                    else
                    {
                        height *= stretch;
                    }
                    var size = new Size(availableSize.Width, height);
                    var measurement = new Measurement(this, size, margin);
                    ms.Add(measurement);
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

            return new TextRun(node.Text ?? string.Empty, new Text.Typeface(node.Element.Font, node.Element.FontSize, node.Element.FontWeight));
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
    }
}
