using Tex.Net.Layout.Document;

namespace Tex.Net.Layout
{
    public class Measurement
    {
        public DocumentElement Element { get; }
        public bool Flexible { get; set; }
        public double PagebreakPenalty { get; set; }
        public Size Size { get; }
        public Thickness Margin { get; }
        public int Index { get; internal set; } = -1;
        public Position Position { get; internal set; }

        public Measurements Subs { get; }

        public Size TotalSize => new Size(Size.Width + Margin.Width, Size.Height + Margin.Height);

        public Measurement(DocumentElement element, Size size, Thickness margin)
        {
            Subs = new Measurements();
            Element = element;
            Size = size;
            Margin = margin;
        }
    }
}
