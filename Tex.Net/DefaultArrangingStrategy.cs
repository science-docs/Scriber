using System;
using System.Collections.Generic;
using System.Linq;
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

            var flexQueue = new FlexQueue<Measurement>(); //new Queue<Measurement>();
            var enqueueNext = false;

            // Stage one:
            // Add measurements to pages
            foreach (var measurement in document.Measurements)
            {
                if (enqueueNext)
                {
                    flexQueue.Enqueue(measurement, measurement.Flexible);
                    enqueueNext = measurement.PagebreakPenalty > 0;
                }
                else if (measurement.PagebreakPenalty > 0)
                {
                    flexQueue.Enqueue(measurement.Flexible);
                    flexQueue.Enqueue(measurement, measurement.Flexible);
                    enqueueNext = true;
                }
                else
                {
                    while (flexQueue.Count > 0)
                    {
                        var peeked = flexQueue.Peek();
                        if (FitAll(peeked, curHeight))
                        {
                            foreach (var ms in peeked)
                            {
                                TryFitMeasurement(curPage, ms, flexQueue, true, ref curHeight);
                            }
                            flexQueue.Dequeue();
                        }
                        else if (!flexQueue.IsFlex)
                        {
                            curHeight = boxSize.Height;
                            curPage.Filled = true;
                            curPage = AddPage(document, pageSize, contentArea);
                        }
                        else
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

                
            }

            // Add leftover flexible items
            while (flexQueue.Count > 0)
            {
                var ms = flexQueue.Dequeue();
                foreach (var m in ms)
                {
                    m.Flexible = false;

                    if (TryFitMeasurement(curPage, m, flexQueue, false, ref curHeight) == FitResult.AddPage)
                    {
                        curHeight = boxSize.Height;
                        curPage.Filled = true;
                        curPage = AddPage(document, pageSize, contentArea);
                    }
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

        private static FitResult TryFitMeasurement(DocumentPage page, Measurement measurement, FlexQueue<Measurement> queue, bool flexMode, ref double height)
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
                        queue.Enqueue(measurement, measurement.Flexible);
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

            return FitResult.Continue;
        }

        private static bool FitAll(IEnumerable<Measurement> measurements, double height)
        {
            return measurements.Sum(e => e.TotalSize.Height) <= height;
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
