using Scriber.Layout.Document;
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

        private readonly Dictionary<(int row, int column), DocumentElement> positions = new Dictionary<(int row, int column), DocumentElement>();

        public Grid()
        {
            Rows = new List<GridLength>();
            Columns = new List<GridLength>();
        }

        public void Set(DocumentElement element, int row, int column)
        {
            Elements.Add(element);
            positions.Add((row, column), element);
        }

        protected override AbstractElement CloneInternal()
        {
            var grid = new Grid();
            grid.Rows.AddRange(Rows);
            grid.Columns.AddRange(Columns);

            foreach (var ((row, column), element) in positions)
            {
                grid.Set(element.Clone(), row, column);
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
                    var cell = new Cell(Rows[i], i, Columns[j], j);
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

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Order == 1)
                {
                    continue;
                }

                var width = cells.Sum(e => e.Size.Width);
                availableSize.Width -= width;
                MeasureCell(ms, cell, availableSize);

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
                    if (cell.Measurement != null)
                    {
                        cell.Measurement.Position = new Position(columnWidth, rowHeight);
                    }

                    columnWidth += columnWidths[j];
                    
                }

                rowHeight += rowHeights[i];
            }

            ms.Size = new Size(columnWidths.Sum(), rowHeight);

            return ms;
        }

        private void MeasureCell(Measurement measurement, Cell cell, Size size)
        {
            if (positions.TryGetValue((cell.RowIndex, cell.ColumnIndex), out var element))
            {
                measurement.Subs.Add(cell.Measure(element, size)!);
            }
            else
            {
                cell.Measure(null, size);
            }
        }

        private class Cell
        {
            public GridLength Row { get; set; }
            public int RowIndex { get; set; }
            public GridLength Column { get; set; }
            public int ColumnIndex { get; set; }

            public Size Size { get; set; }

            public Measurement? Measurement { get; set; }

            public int Order => orders[(Row.Unit, Column.Unit)];

            public Cell(GridLength row, int rowIndex, GridLength column, int columnIndex)
            {
                Row = row;
                RowIndex = rowIndex;
                Column = column;
                ColumnIndex = columnIndex;
            }

            public Measurement? Measure(DocumentElement? element, Size availableSize)
            {
                if (element == null)
                {
                    double y = 0;
                    double x = 0;
                    if (Row.Unit == GridUnit.Point)
                    {
                        y = Row.Value;
                    }
                    else if (Row.Unit == GridUnit.Auto)
                    {
                        y = 0;
                    }
                    else if (Row.Unit == GridUnit.Star)
                    {
                        y = 0;
                    }
                    if (Column.Unit == GridUnit.Point)
                    {
                        x = Column.Value;
                    }
                    else if (Column.Unit == GridUnit.Auto)
                    {
                        x = 0;
                    }
                    else if (Column.Unit == GridUnit.Star)
                    {
                        x = 0;
                    }
                    Size = new Size(x, y);
                    return null;
                }
                else
                {
                    double y = 0;
                    double x = 0;
                    if (Row.Unit == GridUnit.Point)
                    {
                        y = Row.Value;
                    }
                    else if (Row.Unit == GridUnit.Auto)
                    {
                        y = availableSize.Width;
                    }
                    else if (Row.Unit == GridUnit.Star)
                    {
                        y = availableSize.Width;
                    }
                    if (Column.Unit == GridUnit.Point)
                    {
                        x = Column.Value;
                    }
                    else if (Column.Unit == GridUnit.Auto)
                    {
                        x = availableSize.Height;
                    }
                    else if (Column.Unit == GridUnit.Star)
                    {
                        x = availableSize.Height;
                    }

                    Measurement = element.Measure(new Size(x, y));
                    Size = Measurement.Size;

                    if (Row.Unit == GridUnit.Point)
                    {
                        Size = new Size(Size.Width, Row.Value);
                    }
                    else if (Row.Unit == GridUnit.Star)
                    {
                        Size = new Size(Size.Width, availableSize.Height);
                    }

                    if (Column.Unit == GridUnit.Point)
                    {
                        Size = new Size(Column.Value, Size.Height);
                    }
                    else if (Column.Unit == GridUnit.Star)
                    {
                        Size = new Size(availableSize.Width, Size.Height);
                    }

                    return Measurement;
                }
            }

            private static readonly Dictionary<(GridUnit row, GridUnit column), int> orders = new Dictionary<(GridUnit row, GridUnit column), int>
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
