using System.Collections.Generic;
using System.Linq;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber
{
    public class DefaultArrangingStrategy : IArrangingStrategy
    {
        public void Arrange(Document document)
        {
            var pageSize = document.Variable(PageVariables.Size);
            var margin = document.Variable(PageVariables.Margin);
            var boxSize = pageSize;
            boxSize.Height -= margin.Height;
            boxSize.Width -= margin.Width;

            var startOffset = new Position(margin.Left, margin.Top);
            var extraStartOffset = new Position(margin.Left, pageSize.Height - margin.Bottom);
            var contentArea = new Rectangle(startOffset, boxSize);

            MeasuredDocumentPage? curPage = null;
            double curHeight = boxSize.Height;

            var flexQueue = new Queue<Measurement>();
            var measurements = new List<Measurement>(document.Measurements);

            // Stage one:
            // Add measurements to pages
            for (int i = 0; i < measurements.Count; i++)
            {
                var measurement = measurements[i];

                if (measurement.Element is PdfElement pdfElement)
                {
                    document.Pages.Add(new PdfDocumentPage(document, pdfElement));
                    // reseting the page so that following measurements start on a new page.
                    curPage = null;
                }
                else
                {
                    if (curPage == null)
                    {
                        curPage = AddPage(document, pageSize, contentArea);
                        curHeight = boxSize.Height;
                    }

                    FitFlex(curPage, flexQueue, ref curHeight);

                    if (TryFitMeasurement(curPage, measurement, flexQueue, false, ref curHeight) == FitResult.AddPage)
                    {
                        var split = measurement.Element.Split(measurement, curHeight);

                        if (split.IsSplit)
                        {
                            curPage.Measurements.Add(split.Measurement);
                            if (split.Measurement.Element.Page == null)
                            {
                                split.Measurement.Element.Page = curPage;
                            }
                            measurements[i] = split.Measurement;
                            if (split.Next != null)
                            {
                                measurements.Insert(i + 1, split.Next);
                            }
                        }

                        curHeight = boxSize.Height;
                        curPage = AddPage(document, pageSize, contentArea);
                    }
                }
            }

            if (curPage != null)
            {
                // Add leftover flexible items (part of stage one)
                while (flexQueue.Count > 0)
                {
                    var m = flexQueue.Peek();
                    m.Flexible = false;

                    if (TryFitMeasurement(curPage, m, flexQueue, false, ref curHeight) == FitResult.AddPage)
                    {
                        curHeight = boxSize.Height;
                        curPage = AddPage(document, pageSize, contentArea);
                    }
                    else
                    {
                        flexQueue.Dequeue();
                    }
                }
            }

            // Stage two:
            // Layout measurements on each page
            foreach (var page in document.Pages.OfType<MeasuredDocumentPage>())
            {

                var extras = new List<Measurement>();
                var currentOffset = startOffset;
                var extraHeight = 0.0;

                foreach (var measurement in page.Measurements)
                {
                    var accumulated = measurement.AccumulatedExtra;
                    extras.AddRange(accumulated.Subs);
                    extraHeight += accumulated.TotalSize.Height;
                }

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

                var extraOffset = new Position(extraStartOffset.X, extraStartOffset.Y - extraHeight);

                foreach (var extra in extras)
                {
                    var size = extra.Size;

                    extraOffset.Y += extra.Margin.Top;

                    var visualOffset = extraOffset;
                    visualOffset.X += extra.Margin.Left;

                    extra.Position = visualOffset;
                    Realign(extra, boxSize, pageSize, extra.Element.HorizontalAlignment);
                    extra.Element.Arrange(extra);

                    extraOffset.Y += size.Height;
                    extraOffset.Y += extra.Margin.Bottom;
                }

                // Set number at the end of the page layouting
                // so that any number changing elements can take effect
                page.Number = document.PageNumbering.Next();
                page.AddPageItems();
            }
        }

        private static void FitFlex(MeasuredDocumentPage page, Queue<Measurement> queue, ref double height)
        {
            while (queue.Count > 0)
            {
                var peek = queue.Peek();
                var result = TryFitMeasurement(page, peek, queue, true, ref height);
                if (result == FitResult.Stop || result == FitResult.AddPage)
                {
                    break;
                }
                else
                {
                    queue.Dequeue();
                }
            }
        }


        private static FitResult TryFitMeasurement(MeasuredDocumentPage page, Measurement measurement, Queue<Measurement> queue, bool flexMode, ref double height)
        {
            var size = measurement.TotalSize + measurement.AccumulatedExtra.TotalSize;

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

            return FitResult.Continue;
        }

        //private static bool FitAll(IEnumerable<Measurement> measurements, double height)
        //{
        //    return measurements.Sum(e => e.TotalSize.Height) <= height;
        //}

        private static MeasuredDocumentPage AddPage(Document document, Size size, Rectangle contentArea)
        {
            var page = new MeasuredDocumentPage(document)
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
