using PdfSharpCore.Drawing;
using Scriber.Layout;

namespace Scriber.Drawing.Shapes
{
    public class Line : Path
    {
        public Position Start
        {
            get => line.Start;
            set => line.Start = value;
        }
        public Position End
        {
            get => line.End;
            set => line.End = value;
        }

        private readonly LineSegment line;

        public Line() : this(Position.Zero, Position.Zero)
        {
        }

        public Line(Position start, Position end)
        {
            line = new LineSegment
            {
                Start = start,
                End = end
            };
            Segments.Add(line);
        }
    }

    public class LineSegment : PathSegment
    {
        public Position Start { get; set; }
        public Position End { get; set; }

        public override Position StartPoint => Start;

        public override Position EndPoint => End;

        public override void Add(XGraphicsPath path)
        {
            path.AddLine(Start.X, Start.Y, End.X, End.Y);
        }
    }

    public class PointSegment : PathSegment
    {
        public Position Point { get; set; }

        public override Position StartPoint => Position.Zero;

        public override Position EndPoint => Point;

        public override void Add(XGraphicsPath path)
        {
            path.AddLine(0, 0, Point.X, Point.Y);
        }
    }
}
