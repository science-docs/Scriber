using Scriber.Layout.Document;
using Scriber.Layout.Styling;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Scriber.Layout
{
    public class Grid : Panel
    {
        public List<GridLength> Rows { get; }
        public List<GridLength> Columns { get; }

        private readonly Dictionary<(int row, int column), (DocumentElement element, int rowSpan, int columnSpan)> positions = new Dictionary<(int, int), (DocumentElement, int, int)>();

        public Grid()
        {
            Rows = new List<GridLength>();
            Columns = new List<GridLength>();
        }

        public void Set(DocumentElement element, int row, int column, int rowSpan = 1, int columnSpan = 1)
        {
            if (rowSpan < 1)
            {
                throw new ArgumentException("", nameof(rowSpan));
            }
            if (columnSpan < 1)
            {
                throw new ArgumentException("", nameof(columnSpan));
            }

            Elements.Add(element);
            positions.Add((row, column), (element, rowSpan, columnSpan));
        }

        protected override AbstractElement CloneInternal()
        {
            var grid = new Grid();
            grid.Rows.AddRange(Rows);
            grid.Columns.AddRange(Columns);

            foreach (var ((row, column), (element, rowSpan, columnSpan)) in positions)
            {
                grid.Set(element.Clone(), row, column, rowSpan, columnSpan);
            }

            return grid;
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            if (Rows.Count == 0)
            {
                Rows.Add(new GridLength());
            }

            if (Columns.Count == 0)
            {
                Columns.Add(new GridLength());
            }

            var cells = new List<Cell>();
            var cellDictionary = new Dictionary<(int row, int column), Cell>();

            for (int i = 0; i < Rows.Count; i++)
            {
                for (int j = 0; j < Columns.Count; j++)
                {
                    var cell = new Cell(this, i, 1, j, 1);
                    cells.Add(cell);
                    cellDictionary[(i, j)] = cell;
                }
            }
            // See https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/Grid.cs#L459-L607
            cells.Sort((a, b) => a.Order.CompareTo(b.Order));

            var ms = new Measurement(this);

            // measure order 1
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Order != 1)
                {
                    break;
                }

                MeasureCell(ms, cell, availableSize);
                
            }

            // TODO: measure other orders here
            var currentWidth = cells.Sum(e => e.Size.Width);
            var currentHeight = cells.Sum(e => e.Size.Height);
            var restSize = availableSize;
            restSize.Width -= currentWidth;
            restSize.Height -= currentHeight;

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Order == 1)
                {
                    continue;
                }

                MeasureCell(ms, cell, restSize);
            }

            // arrange here
            var rowHeights = new double[Rows.Count];
            var columnWidths = new double[Columns.Count];

            for (int i = 0; i < rowHeights.Length; i++)
            {
                double maxLength = 0;
                for (int j = 0; j < columnWidths.Length; j++)
                {
                    var cell = cellDictionary[(i, j)];
                    maxLength = Math.Max(maxLength, cell.Size.Height);
                }
                rowHeights[i] = maxLength;
            }

            for (int i = 0; i < columnWidths.Length; i++)
            {
                double maxLength = 0;
                for (int j = 0; j < rowHeights.Length; j++)
                {
                    var cell = cellDictionary[(j, i)];
                    maxLength = Math.Max(maxLength, cell.Size.Width);
                }
                columnWidths[i] = maxLength;
            }

            double rowHeight = 0;
            for (int i = 0; i < Rows.Count; i++)
            {
                double columnWidth = 0;
                for (int j = 0; j < Columns.Count; j++)
                {
                    var cell = cellDictionary[(i, j)];
                    cell.Size = new Size(columnWidths[j], rowHeights[i]);
                    if (cell.Measurement != null)
                    {
                        cell.Measurement.Position = new Position(columnWidth, rowHeight);
                        AlignCell(cell);
                    }

                    columnWidth += columnWidths[j];
                    
                }

                rowHeight += rowHeights[i];
            }

            ms.Size = new Size(columnWidths.Sum(), rowHeight);
            ms.Margin = Style.Get(StyleKeys.Margin);

            return ms;
        }

        private void MeasureCell(Measurement measurement, Cell cell, Size size)
        {
            if (positions.TryGetValue((cell.RowIndex, cell.ColumnIndex), out var tuple))
            {
                measurement.Subs.Add(cell.Measure(tuple.element, size)!);
            }
            else
            {
                cell.Measure(null, size);
            }
        }

        private void AlignCell(Cell cell)
        {
            if (cell.Measurement != null)
            {
                var size = cell.Size;
                var ms = cell.Measurement;
                var msSize = ms.TotalSize;
                var el = ms.Element;
                var pos = ms.Position;
                if (msSize != size)
                {
                    if (el.VerticalAlignment == VerticalAlignment.Center)
                    {
                        pos.Y += (size.Height - msSize.Height) / 2;
                    }
                    else if (el.VerticalAlignment == VerticalAlignment.Bottom)
                    {
                        pos.Y += size.Height - msSize.Height;
                    }
                    ms.Position = pos;
                }
            }
        }

        private class Cell
        {
            public Grid Grid { get; }

            public int RowSpan { get; set; }
            public int RowIndex { get; set; }
            public int ColumnSpan { get; set; }
            public int ColumnIndex { get; set; }

            public Size Size { get; set; }

            public Measurement? Measurement { get; set; }

            public int Order => GetOrder();

            public Cell(Grid grid, int rowIndex, int rowSpan, int columnIndex, int columnSpan)
            {
                Grid = grid;
                RowSpan = rowSpan;
                RowIndex = rowIndex;
                ColumnSpan = columnSpan;
                ColumnIndex = columnIndex;
            }

            public Measurement? Measure(DocumentElement? element, Size availableSize)
            {
                var rowUnit = GetUnit(Grid.Rows, RowIndex, RowSpan, out var rowValue);
                var columnUnit = GetUnit(Grid.Columns, ColumnIndex, ColumnSpan, out var columnValue);

                if (element == null)
                {
                    double y = 0;
                    double x = 0;
                    if (rowUnit == GridUnit.Point)
                    {
                        y = rowValue;
                    }
                    else if (rowUnit == GridUnit.Auto)
                    {
                        y = 0;
                    }
                    else if (rowUnit == GridUnit.Star)
                    {
                        y = 0;
                    }
                    if (columnUnit == GridUnit.Point)
                    {
                        x = columnValue;
                    }
                    else if (columnUnit == GridUnit.Auto)
                    {
                        x = 0;
                    }
                    else if (columnUnit == GridUnit.Star)
                    {
                        x = availableSize.Width;
                    }
                    Size = new Size(x, y);
                    return null;
                }
                else
                {
                    double y = 0;
                    double x = 0;
                    if (rowUnit == GridUnit.Point)
                    {
                        y = rowValue;
                    }
                    else if (rowUnit == GridUnit.Auto)
                    {
                        y = availableSize.Height;
                    }
                    else if (rowUnit == GridUnit.Star)
                    {
                        y = availableSize.Height;
                    }
                    if (columnUnit == GridUnit.Point)
                    {
                        x = columnValue;
                    }
                    else if (columnUnit == GridUnit.Auto)
                    {
                        x = availableSize.Width;
                    }
                    else if (columnUnit == GridUnit.Star)
                    {
                        x = availableSize.Width;
                    }

                    x = Math.Max(0, x);
                    y = Math.Max(0, y);

                    Measurement = element.Measure(new Size(x, y));
                    Measurement.Margin = element.Style.Get(StyleKeys.Margin);
                    Size = Measurement.TotalSize;

                    if (rowUnit == GridUnit.Point)
                    {
                        Size = new Size(Size.Width, rowValue);
                    }
                    else if (rowUnit == GridUnit.Star)
                    {
                        Size = new Size(Size.Width, availableSize.Height);
                    }

                    if (columnUnit == GridUnit.Point)
                    {
                        Size = new Size(columnValue, Size.Height);
                    }
                    else if (columnUnit == GridUnit.Star)
                    {
                        Size = new Size(availableSize.Width, Size.Height);
                    }

                    return Measurement;
                }
            }

            private int GetOrder()
            {
                var row = GetUnit(Grid.Rows, RowIndex, RowSpan, out var _);
                var column = GetUnit(Grid.Columns, ColumnIndex, ColumnSpan, out var _);
                return orders[(row, column)];
            }

            private GridUnit GetUnit(List<GridLength> lengths, int index, int span, out double value)
            {
                GridUnit gridUnit = GridUnit.Point;
                value = 0;

                for (int i = 0; i < span; i++)
                {
                    var length = lengths[i + index];
                    if (GreaterThan(length.Unit, gridUnit))
                    {
                        gridUnit = length.Unit;
                        value = 0;
                    }
                    value += length.Value;
                }

                return gridUnit;
            }

            private bool GreaterThan(GridUnit a, GridUnit b)
            {
                if (a == b)
                {
                    return false;
                }

                return a == GridUnit.Auto && b == GridUnit.Point || a == GridUnit.Star && (b == GridUnit.Point || b == GridUnit.Auto);
            }

            private static readonly Dictionary<(GridUnit row, GridUnit column), int> orders = new Dictionary<(GridUnit, GridUnit), int>
            {
                { (GridUnit.Point, GridUnit.Point), 1 }, { (GridUnit.Point, GridUnit.Auto), 1 }, { (GridUnit.Point, GridUnit.Star), 3 },
                { (GridUnit.Auto, GridUnit.Point), 1 }, { (GridUnit.Auto, GridUnit.Auto), 1 }, { (GridUnit.Auto, GridUnit.Star), 3 },
                { (GridUnit.Star, GridUnit.Point), 4 }, { (GridUnit.Star, GridUnit.Auto), 2 }, { (GridUnit.Star, GridUnit.Star), 4 }
            };
        }
    }

    public struct GridLength : IEquatable<GridLength>
    {
        public GridUnit Unit;
        public double Value;

        public GridLength(GridUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public override bool Equals(object? obj) => obj is GridLength length && Equals(length);

        public bool Equals([AllowNull] GridLength other)
        {
            if (other == null)
            {
                return false;
            }
            return Unit == other.Unit && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        public static bool operator ==(GridLength left, GridLength right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridLength left, GridLength right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Unit switch
            {
                GridUnit.Star => $"{Value}*",
                GridUnit.Point => $"{Value}pt",
                _ => nameof(GridUnit.Auto)
            };
        }
    }

    public enum GridUnit
    {
        Auto,
        Star,
        Point
    }
}
