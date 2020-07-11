using Scriber.Layout.Document;

namespace Scriber.Layout
{
    public class StackPanel : Panel
    {
        public Orientation Orientation { get; set; }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            var measurement = new Measurement(this);

            if (Orientation == Orientation.Vertical)
            {
                double y = 0;
                foreach (var child in Elements)
                {
                    var childMeasurement = child.Measure(availableSize);
                    Align(childMeasurement, availableSize, y);
                    y += childMeasurement.TotalSize.Height;
                    measurement.Subs.Add(childMeasurement);
                }
            }
            else
            {
                var ms = new Measurement(this)
                {
                    Size = new Size(availableSize.Width, 0)
                };
                foreach (var child in Elements)
                {
                    var childMeasurement = child.Measure(availableSize);
                    var height = childMeasurement.Size.Height;
                    if (height > ms.Size.Height)
                    {
                        ms.Size = new Size(ms.Size.Width, height);
                    }
                    measurement.Subs.Add(childMeasurement);
                }
                double x = 0;
                foreach (var child in Elements)
                {
                    Align(child.Measurement, ms.Size, x);
                    x += child.Measurement.TotalSize.Width;
                }
                
            }
            
            return measurement;
        }

        private void Align(Measurement measurement, Size availableSize, double value)
        {
            var y = 0.0;
            var x = 0.0;
            var total = measurement.Size;
            if (Orientation == Orientation.Vertical)
            {
                y = value;
                var align = measurement.Element.HorizontalAlignment;
                if (align == HorizontalAlignment.Right)
                {
                    x = availableSize.Width - total.Width;
                }
                else if (align == HorizontalAlignment.Center)
                {
                    x = (availableSize.Width - total.Width) / 2;
                }
            }
            else
            {
                x = value;
                var align = measurement.Element.VerticalAlignment;
                if (align == VerticalAlignment.Bottom)
                {
                    y = availableSize.Height - total.Height;
                }
                else if (align == VerticalAlignment.Center)
                {
                    y = (availableSize.Height - total.Height) / 2;
                }
            }
            measurement.Position = new Position(x, y);
        }

        protected override AbstractElement CloneInternal()
        {
            var panel = new StackPanel
            {
                Orientation = Orientation,
                Flexible = Flexible,
                Glue = Glue
            };
            foreach (var child in Elements)
            {
                panel.Elements.Add(child.Clone());
            }
            return panel;
        }
    }
}
