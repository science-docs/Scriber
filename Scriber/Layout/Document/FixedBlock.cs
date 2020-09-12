using System;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    [Flags]
    public enum FixedPosition
    {
        Top = 1,
        Bottom = 2,

        Left = 4,
        Center = 8,
        Right = 16,

        TopLeft = Top | Left,
        BottomLeft = Bottom | Left,
        TopCenter = Top | Center,
        BottomCenter = Bottom | Center,
        TopRight = Top | Right,
        BottomRight = Bottom | Right
    }

    public class FixedBlock : DocumentElement
    {
        public DocumentElement Child { get; }
        public FixedPosition Position { get; set; } = FixedPosition.TopLeft;

        public FixedBlock(DocumentElement child)
        {
            Child = child;
            Child.Parent = this;
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            foreach (var sub in measurement.Subs)
            {
                Child.Render(drawingContext, sub);
            }
        }

        protected override AbstractElement CloneInternal()
        {
            return new FixedBlock(Child.Clone())
            {
                Position = Position
            };
        }

        public override void Interlude()
        {
            Child.Interlude();
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            var measurement = new Measurement(this);
            var childMeasurement = Child.Measure(availableSize);
            measurement.Subs.Add(childMeasurement);
            measurement.Position = CalculatePosition(childMeasurement.Size, availableSize);
            return measurement;
        }

        private Position CalculatePosition(Size childSize, Size availableSize)
        {
            double x = 0;
            double y = 0;

            if (Position.HasFlag(FixedPosition.Top))
            {
                y = 0;
            }
            else if (Position.HasFlag(FixedPosition.Bottom))
            {
                y = availableSize.Height - childSize.Height;
            }

            if (Position.HasFlag(FixedPosition.Left))
            {
                x = 0;
            }
            else if (Position.HasFlag(FixedPosition.Center))
            {
                x = (availableSize.Width - childSize.Width) / 2;
            }
            else if (Position.HasFlag(FixedPosition.Right))
            {
                x = availableSize.Width - childSize.Width;
            }

            return new Position(x, y);
        }
    }
}
