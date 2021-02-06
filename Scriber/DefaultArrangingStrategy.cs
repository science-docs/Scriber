using System.Collections.Generic;
using System.Linq;
using Scriber.Drawing;
using Scriber.Drawing.Shapes;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Layout.Shapes;
using Scriber.Layout.Styling;
using Scriber.Text;
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

            var flexList = new LinkedList<Measurement>();
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

                    FitFlex(curPage, flexList, ref curHeight);

                    if (TryFitMeasurement(curPage, measurement, flexList, false, ref curHeight) == FitResult.AddPage)
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
                        else if (curHeight == boxSize.Height)
                        {
                            curPage.Measurements.Add(split.Source);
                            if (split.Source.Element.Page == null)
                            {
                                split.Source.Element.Page = curPage;
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
                foreach (var m in flexList)
                {
                    m.Flexible = false;

                    if (TryFitMeasurement(curPage, m, flexList, false, ref curHeight) == FitResult.AddPage)
                    {
                        curHeight = boxSize.Height;
                        curPage = AddPage(document, pageSize, contentArea);
                        TryFitMeasurement(curPage, m, flexList, false, ref curHeight);
                    }
                }
            }
            flexList.Clear();

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
                    var size = measurement.TotalSize;

                    measurement.Position = currentOffset;
                    Realign(measurement, boxSize, pageSize, measurement.Element.Style.Get(StyleKeys.HorizontalAlignment));
                    measurement.Element.Arrange(measurement);

                    currentOffset.Y += size.Height;
                }

                var extraOffset = new Position(extraStartOffset.X, extraStartOffset.Y - extraHeight);

                // Add footnote bar
                if (extras.Count > 0)
                {
                    var line = new Line(Position.Zero, new Position(boxSize.Width / 2.5, 0));
                    var path = new PathElement(line)
                    {
                        Document = document,
                        Page = page,
                        Stroke = Colors.Black,
                        Fill = Colors.Black
                    };
                    var pathMeasurement = path.Measure(boxSize);
                    pathMeasurement.Position = extraOffset;
                    path.Arrange(pathMeasurement);
                    page.Measurements.Add(pathMeasurement);
                }

                foreach (var extra in extras)
                {
                    var size = extra.TotalSize;

                    extra.Position = extraOffset;
                    Realign(extra, boxSize, pageSize, extra.Element.Style.Get(StyleKeys.HorizontalAlignment));
                    extra.Element.Arrange(extra);

                    extraOffset.Y += size.Height;
                }

                // Set number at the end of the page layouting
                // so that any number changing elements can take effect
                page.Number = document.PageNumbering.Next();
                page.AddPageItems();
            }
        }

        private static void FitFlex(MeasuredDocumentPage page, LinkedList<Measurement> flexList, ref double height)
        {
            var head = flexList.First;

            while (head != null)
            {
                var result = TryFitMeasurement(page, head.Value, flexList, true, ref height);
                var previous = head;
                head = head.Next;
                if (result == FitResult.Continue)
                {
                    flexList.Remove(previous);
                }
            }
        }


        private static FitResult TryFitMeasurement(MeasuredDocumentPage page, Measurement measurement, LinkedList<Measurement> flexList, bool flexMode, ref double height)
        {
            var size = measurement.TotalSize + measurement.AccumulatedExtra.TotalSize;

            if (measurement.AccumulatedExtra.Subs.Count > 0)
            {
                height -= 5;
            }

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
                        flexList.AddLast(measurement);
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

        private static MeasuredDocumentPage AddPage(Document document, Size size, Rectangle contentArea)
        {
            var page = new MeasuredDocumentPage(document)
            {
                Size = size,
                ContentArea = contentArea
            };
            page.Index = document.Pages.Count;
            document.Pages.Add(page);
            return page;
        }

        private static void Realign(Measurement measurement, Size pageBox, Size pageSize, HorizontalAlignment alignment)
        {
            var size = measurement.Size;
            Position pos = measurement.Position;

            switch (alignment)
            {
                case HorizontalAlignment.Right:
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
