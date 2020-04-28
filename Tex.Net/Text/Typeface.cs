namespace Tex.Net.Text
{
    public class Typeface
    {
        public Font Font { get; set; }
        public FontWeight Weight { get; set; }
        public double Size { get; set; }

        public Typeface(Font font, double size, FontWeight weight)
        {
            Font = font;
            Size = size;
            Weight = weight;
        }
    }
}
