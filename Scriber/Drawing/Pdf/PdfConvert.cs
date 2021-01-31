using PdfSharpCore.Drawing;
using Scriber.Layout;
using System.Numerics;

namespace Scriber.Drawing.Pdf
{
    internal static class PdfConvert
    {
        public static XBrush ToXBrush(this Color color)
        {
            return new XSolidBrush(XColor.FromArgb(color.Argb));
        }

        public static XColor ToXColor(this Color color)
        {
            return XColor.FromArgb(color.Argb);
        }

        public static XPen ToXPen(this Pen pen)
        {
            return new XPen(ToXColor(pen.Color), pen.StrokeWidth);
        }

        public static XPoint ToXPoint(this Position position)
        {
            return new XPoint(position.X, position.Y);
        }

        public static XRect ToXRect(this Rectangle rectangle)
        {
            return new XRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static XMatrix ToXMatrix(this Matrix3x2 matrix)
        {
            return new XMatrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
        }
    }
}
