namespace Scriber.Text
{
    public class Typeface
    {
        public Font Font { get; set; }
        public FontWeight Weight { get; set; }
        public FontStyle Style { get; set; }
        public double Size { get; set; }

        public Typeface(Font font, double size, FontWeight weight, FontStyle style)
        {
            Font = font;
            Size = size;
            Weight = weight;
            Style = style;
        }
    }
}
