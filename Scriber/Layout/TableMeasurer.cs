using Scriber.Layout.Styling;
using System;
using System.Linq;

namespace Scriber.Layout
{
    public static class TableMeasurer
    {
        public static Measurement Measure(Table table, Size availableSize)
        {
            var measurement = new Measurement(table, null, table.Style.Get(StyleKeys.FullMargin));
            var width = table.Width;

            if (width == 0 || table.Height == 0)
            {
                return measurement;
            }

            double[] desiredColumnSize = new double[width];

            foreach (var row in table.Rows)
            {
                var rowMeasurement = row.Measure(availableSize);

                for (int i = 0; i < rowMeasurement.Subs.Count; i++)
                {
                    var tdSub = rowMeasurement.Subs[i];
                    var currentSize = desiredColumnSize[i];
                    desiredColumnSize[i] = Math.Max(currentSize, tdSub.TotalSize.Width);
                }
            }

            var optimum = FindOptimalColumnWidth(availableSize.Width, desiredColumnSize);

            double y = 0;
            double[] actualColumnSizes = new double[width];
            
            for (int j = 0; j < table.Rows.Count; j++)
            {
                var row = table.Rows[j];
                var rowMeasurement = row.Measurement;

                double x = 0;
                for (int i = 0; i < rowMeasurement.Subs.Count; i++)
                {
                    var columnWidth = desiredColumnSize[i];
                    var sub = rowMeasurement.Subs[i];
                    columnWidth = Math.Min(columnWidth, optimum);
                    sub.Element.Invalidate();
                    var dataMeasurement = sub.Element.Measure(new Size(columnWidth, availableSize.Height));
                    dataMeasurement.Position = new Position(x, 0);
                    x += columnWidth;
                    actualColumnSizes[i] = Math.Max(actualColumnSizes[i], columnWidth);
                    rowMeasurement.Subs[i] = dataMeasurement;
                }

                rowMeasurement.Position = new Position(0, y);

                var maxHeight = rowMeasurement.Subs.Max(e => e.TotalSize.Height);
                y += maxHeight;
                rowMeasurement.Size = new Size(0, maxHeight);
                measurement.Subs.Add(rowMeasurement);
            }

            var actualWidth = actualColumnSizes.Sum();
            foreach (var ms in measurement.Subs)
            {
                ms.Size = new Size(actualWidth, ms.Size.Height);
            }

            return measurement;
        }

        /// <summary>
        /// Finds the optimal maximum column width for a given array of column widths.
        /// Does this by sorting the array and removing the smallest array value from the maximum width
        /// of the full table one by one. For each step we keep track of the average width of the columns.
        /// This value is calculated by dividing the remaining width by the number of leftover columns.
        /// The algorithm ends as soon as the smallest array value becomes larger than the average column width.
        /// The method then returns the average column width of the previous step which is the optimal column width.
        /// </summary>
        /// <param name="fullWidth"></param>
        /// <param name="desiredWidths"></param>
        /// <returns></returns>
        private static double FindOptimalColumnWidth(double fullWidth, double[] desiredWidths)
        {
            var sortedWidths = new double[desiredWidths.Length];
            Array.Copy(desiredWidths, sortedWidths, sortedWidths.Length);
            Array.Sort(sortedWidths);
            int left = desiredWidths.Length;
            double remaining = fullWidth;

            for (int i = 0; i < desiredWidths.Length - 1; i++)
            {
                var currentWidth = sortedWidths[i];
                var nextRemaining = remaining - currentWidth;
                var nextLeft = left - 1;
                var leftOverAverage = nextRemaining / nextLeft;

                if (currentWidth > leftOverAverage)
                {
                    break;
                }

                left = nextLeft;
                remaining = nextRemaining;
            }

            return remaining / left;
        }
    }
}
