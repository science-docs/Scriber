using PdfSharpCore.Drawing;
using Scriber.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Drawing.Shapes
{
    public abstract class PathSegment
    {
        public abstract Position StartPoint { get; }
        public abstract Position EndPoint { get; }

        public abstract void Add(XGraphicsPath path);
    }
}
