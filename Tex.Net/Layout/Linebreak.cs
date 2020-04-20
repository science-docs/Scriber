using System;
using System.Collections.Generic;
using Tex.Net.Layout.Document;
using Tex.Net.Text;

namespace Tex.Net.Layout
{
    public class Breakpoint
    {
        public int Index { get; set; }
        public double Demerits { get; set; }
        public double Ratio { get; set; }
        public int Line { get; set; }
        public int FitnessClass { get; set; }
        public double TotalWidth { get; set; }
        public double TotalStretch { get; set; }
        public double TotalShrink { get; set; }
        public Breakpoint Previous { get; set; }
    }

    public class LineNode
    {
        public LineNodeType Type { get; set; }
        public double Width { get; set; }
        public DocumentElement Element { get; set; }
        public string Text { get; set; }
        public double Stretch { get; set; }
        public double Shrink { get; set; }
        public double Cost { get; set; }
        public bool Flagged { get; set; }

        public static LineNode None(double width, double stretch, double shrink)
        {
            return new LineNode
            {
                Width = width,
                Stretch = stretch,
                Shrink = shrink
            };
        }

        public static LineNode Glue(double width, double stretch, double shrink)
        {
            return new LineNode
            {
                Type = LineNodeType.Glue,
                Width = width,
                Stretch = stretch,
                Shrink = shrink
            };
        }

        public static LineNode Box(double width, string value)
        {
            return new LineNode
            {
                Type = LineNodeType.Box,
                Width = width,
                Text = value
            };
        }

        public static LineNode Penalty(double width, double cost, bool flagged)
        {
            return new LineNode
            {
                Type = LineNodeType.Penalty,
                Width = width,
                Cost = cost,
                Flagged = flagged
            };
        }

        public static LineNode ForceBreak()
        {
            return Penalty(0, -Linebreak.Infinity, false);
        }
    }

    public enum LineNodeType
    {
        None,
        Glue,
        Box,
        Penalty
    }

    public class LinebreakOptions
    {
        public double? MaxAdjustmentRatio { get; set; } = null;
        public double InitialMaxAdjustmentRatio { get; set; } = 1;
        public double DoubleHyphenPenalty { get; set; } = 0;
        public double AdjacentLooseTightPenalty { get; set; } = 0;
    }

    public class PositionedItem
    {
        public int Index { get; }
        public int Line { get; }
        public double Offset { get; }
        public double Width { get; }

        public PositionedItem(int index, int line, double offset, double width)
        {
            Index = index;
            Line = line;
            Offset = offset;
            Width = width;
        }
    }

    public class LinebreakException : Exception
    {
        public int NodeIndex { get; set; } = -1;

        public LinebreakException(string message) : base(message)
        {
        }

        public LinebreakException(string message, int index) : base(message)
        {
            NodeIndex = index;
        }

        public LinebreakException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LinebreakException() : this("An unknown exception occured during the line breaking process")
        {
        }
    }

    public class Linebreak
    {
        public const double Infinity = 1000.0;

        private const double MinAdjustmentRatio = -1;

        private bool IsForcedBreak(LineNode node)
        {
            return node.Type == LineNodeType.Penalty && node.Cost <= -Infinity;
        }

        public int[] BreakLines(List<LineNode> nodes, double[] lineLengths, LinebreakOptions options)
        {
            if (nodes.Count == 0)
            {
                return Array.Empty<int>();
            }

            var currentMaxAdjustmentRatio = Math.Min(options.InitialMaxAdjustmentRatio, options.MaxAdjustmentRatio ?? Infinity);

            var active = new List<Breakpoint>
            {
                // Add initial active node for beginning of paragraph.
                new Breakpoint()
            };

            var sumWidth = 0.0;
            var sumStretch = 0.0;
            var sumShrink = 0.0;

            var minAdjustmentRatioAboveThreshold = Infinity;

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                if (node.Width < 0)
                {
                    throw new LinebreakException($"Node at index {i} has negative width", i);
                }

                var canBreak = false;
                if (node.Type == LineNodeType.Box)
                {
                    sumWidth += node.Width;
                }
                else if (node.Type == LineNodeType.Glue)
                {
                    if (node.Shrink < 0 || node.Stretch < 0)
                    {
                        throw new LinebreakException($"Node at index {i} has negative shrink or stretch", i);
                    }

                    canBreak = i > 0 && nodes[i - 1].Type == LineNodeType.Box;

                    if (!canBreak)
                    {
                        sumWidth += node.Width;
                        sumShrink += node.Shrink;
                        sumStretch = node.Stretch;
                    }
                }
                else if (node.Type == LineNodeType.Penalty)
                {
                    canBreak = node.Cost < Infinity;
                }

                if (!canBreak)
                {
                    continue;
                }

                Breakpoint lastActive = null;
                List<Breakpoint> feasible = new List<Breakpoint>();

                for (int j = 0; j < active.Count; j++)
                {
                    var breakpoint = active[j];
                    var lineShrink = sumShrink - breakpoint.TotalShrink;
                    var lineStretch = sumStretch - breakpoint.TotalStretch;
                    var idealLength = IdealLength(lineLengths, breakpoint.Line);
                    var actualLength = sumWidth - breakpoint.TotalWidth;

                    if (node.Type == LineNodeType.Penalty)
                    {
                        actualLength += node.Width;
                    }

                    double adjustmentRatio;
                    if (actualLength < idealLength)
                    {
                        adjustmentRatio = (idealLength - actualLength) / lineStretch;
                    }
                    else
                    {
                        adjustmentRatio = (idealLength - actualLength) / lineShrink;
                    }

                    if (adjustmentRatio > currentMaxAdjustmentRatio)
                    {
                        // In case we need to try again later with a higher
                        // `maxAdjustmentRatio`, track the minimum value needed to produce
                        // different output.
                        minAdjustmentRatioAboveThreshold = Math.Min(adjustmentRatio, minAdjustmentRatioAboveThreshold);
                    }

                    if (adjustmentRatio < MinAdjustmentRatio || IsForcedBreak(node))
                    {
                        // Items from 'a' to 'b' cannot fit on one line.
                        active.RemoveAt(j--);
                        lastActive = breakpoint;
                    }

                    if (adjustmentRatio >= MinAdjustmentRatio && adjustmentRatio <= currentMaxAdjustmentRatio)
                    {
                        // We found a feasible breakpoint. Compute a 'demerits' score for it
                        double demerits;
                        var badness = 100 * Math.Pow(Math.Abs(adjustmentRatio), 3);
                        var penalty = node.Type == LineNodeType.Penalty ? node.Cost : 0;

                        if (penalty >= 0)
                        {
                            demerits = Math.Pow(1 + badness + penalty, 2);
                        }
                        else if (penalty > -Infinity)
                        {
                            demerits = Math.Pow(1 + badness, 2) - Math.Pow(penalty, 2);
                        }
                        else
                        {
                            demerits = Math.Pow(1 + badness, 2);
                        }

                        var doubleHypenPenalty = 0.0;
                        var previous = nodes[breakpoint.Index];

                        if (node.Type == LineNodeType.Penalty && previous.Type == LineNodeType.Penalty && node.Flagged && previous.Flagged)
                        {
                            doubleHypenPenalty = options.DoubleHyphenPenalty;
                        }

                        demerits += doubleHypenPenalty;

                        int fitness;
                        if (adjustmentRatio < -0.5)
                            fitness = 0;
                        else if (adjustmentRatio < 0.5)
                            fitness = 1;
                        else if (adjustmentRatio < 1)
                            fitness = 2;
                        else
                            fitness = 3;

                        if (breakpoint.Index > 0 && Math.Abs(fitness - breakpoint.FitnessClass) > 1)
                        {
                            demerits += options.AdjacentLooseTightPenalty;
                        }

                        var widthToNextBox = 0.0;
                        var shrinkToNextBox = 0.0;
                        var stretchToNextBox = 0.0;
                        for (int k = i; k < nodes.Count; k++)
                        {
                            var item = nodes[k];
                            if (item.Type == LineNodeType.Box || (item.Type == LineNodeType.Penalty && item.Cost >= Infinity))
                            {
                                break;
                            }
                            widthToNextBox += item.Width;
                            if (item.Type == LineNodeType.Glue)
                            {
                                shrinkToNextBox += item.Shrink;
                                stretchToNextBox += item.Stretch;
                            }
                        }

                        var nextBreakpoint = new Breakpoint
                        {
                            Index = i,
                            Line = breakpoint.Line + 1,
                            FitnessClass = fitness,
                            TotalWidth = sumWidth + widthToNextBox,
                            TotalShrink = sumShrink + shrinkToNextBox,
                            TotalStretch = sumStretch + stretchToNextBox,
                            Demerits = breakpoint.Demerits + demerits,
                            Previous = breakpoint
                        };

                        feasible.Add(nextBreakpoint);
                    }
                }

                var bestNode = FindBest(feasible);
                if (bestNode != null)
                {
                    active.Add(bestNode);
                }

                if (active.Count == 0)
                {
                    if (minAdjustmentRatioAboveThreshold < Infinity)
                    {
                        if (options.MaxAdjustmentRatio == currentMaxAdjustmentRatio)
                        {
                            throw new LinebreakException("Max adjustment exceeded", i);
                        }

                        var adjustedOptions = new LinebreakOptions
                        {
                            AdjacentLooseTightPenalty = options.AdjacentLooseTightPenalty,
                            DoubleHyphenPenalty = options.DoubleHyphenPenalty,
                            MaxAdjustmentRatio = options.MaxAdjustmentRatio,
                            InitialMaxAdjustmentRatio = minAdjustmentRatioAboveThreshold * 2
                        };

                        return BreakLines(nodes, lineLengths, adjustedOptions);
                    }
                    else
                    {
                        active.Add(new Breakpoint
                        {
                            Index = i,
                            Line = lastActive.Line + 1,
                            FitnessClass = 1,
                            TotalWidth = sumWidth,
                            TotalShrink = sumShrink,
                            TotalStretch = sumStretch,
                            Demerits = lastActive.Demerits + Infinity,
                            Previous = lastActive
                        });
                    }
                }

                if (node.Type == LineNodeType.Glue)
                {
                    sumWidth += node.Width;
                    sumStretch += node.Stretch;
                    sumShrink += node.Shrink;
                }
            }

            var actualBestNode = FindBest(active);

            var output = new List<int>();

            var next = actualBestNode;

            while (next != null)
            {
                output.Add(next.Index);
                next = next.Previous;
            }

            output.Reverse();

            return output.ToArray();
        }

        private Breakpoint FindBest(List<Breakpoint> breakpoints)
        {
            if (breakpoints.Count > 0)
            {
                var bestNode = breakpoints[0];
                for (int i = 1; i < breakpoints.Count; i++)
                {
                    if (breakpoints[i].Demerits < bestNode.Demerits)
                    {
                        bestNode = breakpoints[i];
                    }
                }
                return bestNode;
            }
            return null;
        }

        private double IdealLength(double[] lineLength, int index)
        {
            return index < lineLength.Length ? lineLength[index] : lineLength[^1];
        }

        private double[] AdjustmentRatios(List<LineNode> nodes, double[] lineLengths, int[] breakpoints)
        {
            // Create one extra item that will be 0 
            // as the last line does not require adjustment
            var ratios = new double[breakpoints.Length];

            for (int i = 0; i < breakpoints.Length - 1; i++)
            {
                var idealWidth = IdealLength(lineLengths, i);
                var actualWidth = 0.0;
                var lineShrink = 0.0;
                var lineStretch = 0.0;

                var start = i == 0 ? breakpoints[i] : breakpoints[i] + 1;

                for (int j = start; j <= breakpoints[i + 1]; j++)
                {
                    var item = nodes[j];
                    if (item.Type == LineNodeType.Box)
                    {
                        actualWidth += item.Width;
                    }
                    else if (item.Type == LineNodeType.Glue && j != start && j != breakpoints[i + 1])
                    {
                        actualWidth += item.Width;
                        lineShrink += item.Shrink;
                        lineStretch += item.Stretch;
                    }
                    else if (item.Type == LineNodeType.Penalty && j == breakpoints[i + 1])
                    {
                        actualWidth += item.Width;
                    }
                }

                double adjustmentRatio;
                if (actualWidth < idealWidth)
                {
                    adjustmentRatio = (idealWidth - actualWidth) / lineStretch;
                }
                else
                {
                    adjustmentRatio = (idealWidth - actualWidth) / lineShrink;
                }

                ratios[i] = adjustmentRatio;
            }

            return ratios;
        }

        public PositionedItem[] PositionItems(List<LineNode> nodes, double[] lineLengths, int[] breakpoints, bool includeGlue)
        {
            var adjRatios = AdjustmentRatios(nodes, lineLengths, breakpoints);
            var result = new List<PositionedItem>();

            for (int i = 0; i < breakpoints.Length; i++)
            {
                // Limit the amount of shrinking of lines to 1x `glue.shrink` for each glue
                // item in a line.
                var adjRatio = Math.Max(adjRatios[i], MinAdjustmentRatio);
                var offset = 0.0;
                var start = i == 0 ? breakpoints[i] : breakpoints[i] + 1;
                // When the last line is reached, simply use the node count, otherwise the breakpoints
                var next = i == breakpoints.Length - 1 ? nodes.Count - 1 : breakpoints[i + 1];

                for (int j = start; j <= next; j++)
                {
                    var item = nodes[j];
                    if (item.Type == LineNodeType.Box)
                    {
                        result.Add(new PositionedItem(j, i, offset, item.Width));
                        offset += item.Width;
                    }
                    else if (item.Type == LineNodeType.Glue && j != start && j != next)
                    {
                        double gap = item.Width + adjRatio * (adjRatio < 0 ? item.Shrink : item.Stretch);

                        if (includeGlue)
                        {
                            result.Add(new PositionedItem(j, i, offset, gap));
                        }
                        offset += gap;
                    }
                    else if (item.Type == LineNodeType.Penalty && j == next && item.Width > 0)
                    {
                        result.Add(new PositionedItem(j, i, offset, item.Width));
                    }
                }
            }

            return result.ToArray();
        }
    }
}
