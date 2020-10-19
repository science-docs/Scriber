using PdfSharpCore.Drawing;
using Scriber.Layout;
using System;
using System.Collections.Generic;

namespace Scriber.Drawing.Shapes
{
    public class Path
    {
        public List<PathSegment> Segments { get; } = new List<PathSegment>();

        public Size Size
        {
            get
            {
                var gx = 0.0;
                var gy = 0.0;
                var lx = 0.0;
                var ly = 0.0;
                var point = Position.Zero;
                var first = true;

                foreach (var segment in Segments)
                {
                    if (first)
                    {
                        point = segment.StartPoint;
                        gx = point.X;
                        gy = point.Y;
                        lx = point.X;
                        ly = point.Y;
                        first = false;
                    }

                    var relPoint = segment.EndPoint - segment.StartPoint;
                    point += relPoint;
                    gx = Math.Max(point.X, gx);
                    gy = Math.Max(point.Y, gy);
                    lx = Math.Min(point.X, lx);
                    ly = Math.Min(point.Y, ly);
                }

                return new Size(gx - lx, gy - ly);
            }
        }

        public XGraphicsPath ToPdfPath()
        {
            var path = new XGraphicsPath();
            foreach (var segment in Segments)
            {
                segment.Add(path);
            }
            return path;
        }
    }
}
