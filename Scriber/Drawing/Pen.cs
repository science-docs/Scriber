using Scriber.Text;

namespace Scriber.Drawing
{
    public class Pen
    {
        public Color Color { get; set; }
        public double StrokeWidth { get; set; }

        public Pen(Color color, double strokeWidth)
        {
            Color = color;
            StrokeWidth = strokeWidth;
        }
    }
}
