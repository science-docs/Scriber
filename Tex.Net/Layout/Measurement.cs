using Tex.Net.Layout.Document;

namespace Tex.Net.Layout
{
    public class Measurement
    {
        public DocumentElement Element { get; }
        public bool Flexible { get; set; }
        public double PagebreakPenalty { get; set; }
        public Size Size { get; set; }
        public Thickness Margin { get; set; }
        public int Index { get; internal set; } = -1;
        public Position Position { get; internal set; }

        public Measurements Subs { get; }

        public Size TotalSize => new Size(Size.Width + Margin.Width, Size.Height + Margin.Height);

        public Measurement(DocumentElement element) : this(element, Size.Zero, Thickness.Zero)
        {
        }

        public Measurement(DocumentElement element, Size size, Thickness margin)
        {
            Subs = new Measurements();
            Element = element;
            Size = size;
            Margin = margin;
        }
    }
}
