using System;
using System.Collections.Generic;
using Tex.Net.Layout;

namespace Tex.Net
{
    public class DefaultArrangingStrategy : IArrangingStrategy
    {
        public void Arrange(Document document)
        {
            var pageSize = document.Variables["page"]["size"].GetValue<Size>();
            var margin = document.Variables["page"]["margin"].GetValue<Thickness>();
            var boxSize = pageSize;
            boxSize.Height -= margin.Height;
            boxSize.Width -= margin.Width;

            var startOffset = new Position(margin.Left, margin.Top);
            var contentArea = new Rectangle(startOffset, boxSize);

            var curPage = AddPage(document, pageSize, contentArea);
            double curHeight = boxSize.Height;

            var flexQueue = new Queue<Measurement>();
            var lastPenalty = .0;

            // Stage one:
            // Add measurements to pages
            foreach (var measurement in document.Measurements)
            {
                if (lastPenalty > 0)
                {

                }

                while (flexQueue.Count > 0)
                {
                    var result = TryFitMeasurement(curPage, flexQueue.Peek(), flexQueue, true, ref curHeight);
                    if (result == FitResult.Stop)
                    {
                        break;
                    }
                }

                if (TryFitMeasurement(curPage, measurement, flexQueue, false, ref curHeight) == FitResult.AddPage)
                {
                    curHeight = boxSize.Height;
                    curPage.Filled = true;
                    curPage = AddPage(document, pageSize, contentArea);
                }
            }

            // Add leftover flexible items
            while (flexQueue.Count > 0)
            {
                var ms = flexQueue.Dequeue();
                ms.Flexible = false;

                if (TryFitMeasurement(curPage, ms, flexQueue, false, ref curHeight) == FitResult.AddPage)
                {
                    curHeight = boxSize.Height;
                    curPage.Filled = true;
                    curPage = AddPage(document, pageSize, contentArea);
                }
            }

            // Stage two:
            // Layout measurements on each page
            foreach (var page in document.Pages)
            {
                var currentOffset = startOffset;

                foreach (var measurement in page.Measurements)
                {
                    var size = measurement.Size;

                    currentOffset.Y += measurement.Margin.Top;

                    var visualOffset = currentOffset;
                    visualOffset.X += measurement.Margin.Left;

                    measurement.Position = visualOffset;
                    Realign(measurement, boxSize, pageSize, measurement.Element.HorizontalAlignment);
                    measurement.Element.Arrange(measurement);

                    currentOffset.Y += size.Height;
                    currentOffset.Y += measurement.Margin.Bottom;
                }

                // Set number at the end of the page layouting
                // so that any number changing elements can take effect
                page.Number = document.PageNumbering.Next();
                page.AddPageItems();
            }
        }

        private static FitResult TryFitMeasurement(DocumentPage page, Measurement measurement, Queue<Measurement> queue, bool flexMode, ref double height)
        {
            var size = measurement.TotalSize;

            if (size.Height > height)
            {
                if (measurement.Flexible)
                {
                    if (flexMode)
                    {
                        return FitResult.Stop;
                    }
                    else
                    {
                        queue.Enqueue(measurement);
                        return FitResult.Enqueue;
                    }
                }
                else
                {
                    return FitResult.AddPage;
                }
            }

            if (measurement.Element.Page == null)
            {
                measurement.Element.Page = page;
            }

            height -= size.Height;
            page.Measurements.Add(measurement);

            if (flexMode)
            {
                queue.Dequeue();
            }

            return FitResult.Continue;
        }

        private static DocumentPage AddPage(Document document, Size size, Rectangle contentArea)
        {
            var page = new DocumentPage(document)
            {
                Size = size,
                ContentArea = contentArea
            };

            document.Pages.Add(page);
            return page;
        }

        private static void Realign(Measurement measurement, Size pageBox, Size pageSize, HorizontalAlignment alignment)
        {
            var size = measurement.Size;
            Position pos = measurement.Position;

            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    pos.X = pageBox.Width - size.Width;
                    break;
                case HorizontalAlignment.Center:
                    pos.X = (pageSize.Width - size.Width) / 2;
                    break;
            }

            measurement.Position = pos;
        }

        private enum FitResult
        {
            Continue,
            AddPage,
            Enqueue,
            Stop
        }
    }
}
