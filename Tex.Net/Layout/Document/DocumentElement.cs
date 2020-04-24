using System.Drawing;
using Tex.Net.Text;

namespace Tex.Net.Layout.Document
{
    public abstract class DocumentElement
    {
        public DocumentElement Parent { get; set; }

        public Font Font
        {
            get => font ?? Parent?.Font;
            set => font = value;
        }
        public double FontSize
        {
            get => fontSize ?? Parent?.FontSize ?? 0;
            set => fontSize = value;
        }
        public FontWeight FontWeight
        {
            get => fontWeight ?? Parent?.FontWeight ?? FontWeight.Normal;
            set => fontWeight = value;
        }
        public Color Foreground
        {
            get => foreground ?? Parent?.Foreground ?? Color.Black;
            set => foreground = value;
        }

        public Alignment Alignment
        {
            get => alignment ?? Parent?.Alignment ?? 0;
            set => alignment = value;
        }

        public DocumentPage Page
        {
            get => page ?? Parent?.Page;
            set => page = value;
        }

        public Net.Document Document
        {
            get => document ?? Parent?.Document;
            set => document = value;
        }

        public Thickness Margin { get; set; } = Thickness.Zero;
        
        private Font font;
        private double? fontSize;
        private Color? foreground;
        private FontWeight? fontWeight;
        private Alignment? alignment;
        private DocumentPage page;
        private Net.Document document;

        public DocumentElement Clone()
        {
            var clone = CloneInternal();
            clone.Margin = Margin;
            clone.font = font;
            clone.fontSize = fontSize;
            clone.foreground = foreground;
            clone.fontWeight = fontWeight;
            clone.document = document;
            clone.alignment = alignment;
            return clone;
        }

        protected abstract DocumentElement CloneInternal();

        public virtual void Interlude()
        {

        }
    }
}
