using System;
using System.Collections.Generic;
using Scriber.Drawing;
using Scriber.Text;
using Scriber.Variables;

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

        public override IEnumerable<Symbol> Symbols
        {
            get
            {
                var childSymbols = Child.Symbols;
                foreach (var symbol in childSymbols)
                {
                    symbol.Type = SymbolType.Header;
                }
                return childSymbols;
            }
        }

        public FixedBlock(DocumentElement child)
        {
            Child = child;
            Child.Parent = this;
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            yield return Child;
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
            var margin = Document?.Variable(PageVariables.Margin) ?? Thickness.Zero;

            double x = 0;
            double y = 0;

            if (Position.HasFlag(FixedPosition.Top))
            {
                y = Math.Max((margin.Top - childSize.Height) / 2, 0);
            }
            else if (Position.HasFlag(FixedPosition.Bottom))
            {
                y = availableSize.Height - Math.Max((margin.Bottom + childSize.Height) / 2, childSize.Height);
            }

            if (Position.HasFlag(FixedPosition.Left))
            {
                x = margin.Left;
            }
            else if (Position.HasFlag(FixedPosition.Center))
            {
                x = (availableSize.Width - childSize.Width) / 2;
            }
            else if (Position.HasFlag(FixedPosition.Right))
            {
                x = availableSize.Width - childSize.Width - margin.Right;
            }

            return new Position(x, y);
        }
    }
}
